using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MDMUI.Model; // 引入 Model 命名空间
using MDMUI.Utility; // 假设 DBHelper 在这里

namespace MDMUI.DAL
{
    /// <summary>
    /// 设备组数据访问类
    /// </summary>
    public class EqpGroupDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 获取设备组列表 (可按类型和ID筛选)
        /// </summary>
        /// <param name="eqpGroupType">设备组类型 (可选, null或空表示不筛选)</param>
        /// <param name="eqpGroupId">设备组ID (可选, null或空表示不筛选)</param>
        /// <returns>包含设备组信息的DataTable</returns>
        public DataTable GetEqpGroupList(string eqpGroupType = null, string eqpGroupId = null)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"SELECT 
                            eg.eqp_group_id, 
                            eg.eqp_group_type, 
                            eg.eqp_group_description, 
                            eg.factory_id, 
                            f.FactoryName,  -- 加入工厂名称
                            eg.event_user, 
                            eg.event_remark, 
                            eg.edit_time, 
                            eg.create_time, 
                            eg.event_type 
                          FROM eqp_group eg
                          LEFT JOIN Factory f ON eg.factory_id = f.FactoryId -- 左连接工厂表
                          WHERE 1=1";

            if (!string.IsNullOrEmpty(eqpGroupType))
            {
                sql += " AND eg.eqp_group_type = @EqpGroupType";
                parameters.Add(new SqlParameter("@EqpGroupType", eqpGroupType));
            }

            if (!string.IsNullOrEmpty(eqpGroupId))
            {
                sql += " AND eg.eqp_group_id LIKE @EqpGroupId";
                // 使用 LIKE 进行模糊搜索
                parameters.Add(new SqlParameter("@EqpGroupId", "%" + eqpGroupId + "%")); 
            }

            sql += " ORDER BY eg.create_time DESC"; // 按创建时间降序排序

            try
            {
                 // 移除 DBHelper 调用
                 // dt = DBHelper.GetDataTable(sql, parameters.ToArray()); 
                 using (SqlConnection conn = new SqlConnection(connectionString))
                 {
                     conn.Open();
                     using (SqlCommand cmd = new SqlCommand(sql, conn))
                     {
                         if (parameters.Count > 0)
                         {
                             cmd.Parameters.AddRange(parameters.ToArray());
                         }
                         using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                         {
                             adapter.Fill(dt);
                         }
                     }
                 }
            }
            catch (Exception ex)
            {
                // 在实际应用中应该记录日志
                Console.WriteLine("Error getting EqpGroup list: " + ex.Message);
                throw; // 重新抛出异常，让上层处理或知道发生了错误
            }

            return dt;
        }
        
        /// <summary>
        /// 根据设备组ID获取其历史记录
        /// </summary>
        /// <param name="eqpGroupId">设备组ID</param>
        /// <returns>包含设备组历史记录的DataTable</returns>
        public DataTable GetEqpGroupHisList(string eqpGroupId)
        {
            DataTable dt = new DataTable();
            string sql = @"SELECT 
                            h.id AS Id, 
                            h.eqp_group_id AS EqpGroupId, 
                            h.eqp_group_type AS EqpGroupType, 
                            h.eqp_group_description AS EqpGroupDescription, 
                            h.factory_id AS FactoryId, 
                            f.FactoryName, 
                            h.event_user AS EventUser, 
                            h.event_remark AS EventRemark, 
                            h.edit_time AS EditTime, 
                            h.create_time AS CreateTime, 
                            h.event_type AS EventType 
                          FROM eqp_group_his h
                          LEFT JOIN Factory f ON h.factory_id = f.FactoryId -- 左连接工厂表
                          WHERE h.eqp_group_id = @EqpGroupId 
                          ORDER BY h.create_time DESC"; // 按记录创建时间降序排序

            SqlParameter[] parameters = {
                new SqlParameter("@EqpGroupId", eqpGroupId)
            };

            try
            {
                // 移除 DBHelper 调用
                // dt = DBHelper.GetDataTable(sql, parameters);
                 using (SqlConnection conn = new SqlConnection(connectionString))
                 {
                     conn.Open();
                     using (SqlCommand cmd = new SqlCommand(sql, conn))
                     {
                         cmd.Parameters.AddRange(parameters);
                         using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                         {
                             adapter.Fill(dt);
                         }
                     }
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting EqpGroup history: " + ex.Message);
                throw;
            }

            return dt;
        }

        // --- 这里需要补充 Add, Update, Delete 设备组的方法 --- 
        // --- 以及记录历史到 eqp_group_his 表的方法 --- 

        /// <summary>
        /// 添加设备组，并记录历史
        /// </summary>
        public bool AddEqpGroup(EqpGroup group, string eventUser)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. 插入主表记录
                        string insertSql = @"INSERT INTO eqp_group 
                                             (eqp_group_id, eqp_group_type, eqp_group_description, factory_id, 
                                              event_user, event_remark, edit_time, event_type)
                                           VALUES 
                                             (@EqpGroupId, @EqpGroupType, @EqpGroupDescription, @FactoryId, 
                                              @EventUser, @EventRemark, @EditTime, @EventType)";
                        DateTime now = DateTime.Now;
                        using (SqlCommand insertCmd = new SqlCommand(insertSql, conn, transaction))
                        {
                            insertCmd.Parameters.AddWithValue("@EqpGroupId", group.EqpGroupId);
                            insertCmd.Parameters.AddWithValue("@EqpGroupType", group.EqpGroupType);
                            insertCmd.Parameters.AddWithValue("@EqpGroupDescription", (object)group.EqpGroupDescription ?? DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@FactoryId", (object)group.FactoryId ?? DBNull.Value);
                            insertCmd.Parameters.AddWithValue("@EventUser", eventUser);
                            insertCmd.Parameters.AddWithValue("@EventRemark", "设备组创建"); // 备注
                            insertCmd.Parameters.AddWithValue("@EditTime", now); // 编辑时间也记录为当前时间
                            insertCmd.Parameters.AddWithValue("@EventType", "Create"); // 操作类型

                            int rowsAffected = insertCmd.ExecuteNonQuery();
                            if (rowsAffected == 0) 
                            {
                                transaction.Rollback();
                                return false; // 插入失败
                            }
                        }

                        // 2. 插入历史记录
                        // 注意：历史记录中的 create_time 是历史记录本身的创建时间，edit_time 是操作发生时间
                        // 如果需要原记录的创建时间，需要在历史表添加字段并在删除/更新时传递
                        string insertHisSql = @"INSERT INTO eqp_group_his 
                                                (eqp_group_id, eqp_group_type, eqp_group_description, factory_id, 
                                                 event_user, event_remark, edit_time, event_type)
                                              VALUES 
                                                (@EqpGroupId, @EqpGroupType, @EqpGroupDescription, @FactoryId, 
                                                 @EventUser, @EventRemark, @EditTime, @EventType)";
                        using (SqlCommand insertHisCmd = new SqlCommand(insertHisSql, conn, transaction))
                        {
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupId", group.EqpGroupId);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupType", group.EqpGroupType);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupDescription", (object)group.EqpGroupDescription ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@FactoryId", (object)group.FactoryId ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@EventUser", eventUser); 
                            insertHisCmd.Parameters.AddWithValue("@EventRemark", "设备组创建"); 
                            insertHisCmd.Parameters.AddWithValue("@EditTime", now); // 操作时间
                            insertHisCmd.Parameters.AddWithValue("@EventType", "Create");

                            insertHisCmd.ExecuteNonQuery();
                        }

                        transaction.Commit(); // 提交事务
                        return true;

                    }
                    catch (SqlException ex) when (ex.Number == 2627) // Primary Key violation
                    {
                        transaction.Rollback();
                        // 可以记录日志
                        throw new Exception($"添加设备组失败：设备组ID '{group.EqpGroupId}' 已存在。", ex);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // 发生异常时回滚
                        Console.WriteLine("Error adding EqpGroup: " + ex.Message);
                        // 可以选择记录日志 LogHelper.Log(...)
                        throw; // 重新抛出异常，让上层处理
                    }
                }
            }
        }

        /// <summary>
        /// 更新设备组，并记录历史
        /// </summary>
        public bool UpdateEqpGroup(EqpGroup group, string eventUser)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. 更新主表记录
                        string updateSql = @"UPDATE eqp_group SET 
                                                eqp_group_type = @EqpGroupType, 
                                                eqp_group_description = @EqpGroupDescription, 
                                                factory_id = @FactoryId, 
                                                event_user = @EventUser, 
                                                event_remark = @EventRemark, 
                                                edit_time = @EditTime, 
                                                event_type = @EventType 
                                             WHERE eqp_group_id = @EqpGroupId";
                        DateTime now = DateTime.Now;
                        using (SqlCommand updateCmd = new SqlCommand(updateSql, conn, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@EqpGroupType", group.EqpGroupType);
                            updateCmd.Parameters.AddWithValue("@EqpGroupDescription", (object)group.EqpGroupDescription ?? DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@FactoryId", (object)group.FactoryId ?? DBNull.Value);
                            updateCmd.Parameters.AddWithValue("@EventUser", eventUser);
                            updateCmd.Parameters.AddWithValue("@EventRemark", "设备组更新"); // 备注
                            updateCmd.Parameters.AddWithValue("@EditTime", now);
                            updateCmd.Parameters.AddWithValue("@EventType", "Update"); // 操作类型
                            updateCmd.Parameters.AddWithValue("@EqpGroupId", group.EqpGroupId); // Where 条件

                            int rowsAffected = updateCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                transaction.Rollback();
                                // Consider logging this situation or throwing a specific exception
                                // e.g., throw new KeyNotFoundException($"Update failed: EqpGroup with ID '{group.EqpGroupId}' not found.");
                                return false; // 更新失败，可能记录不存在
                            }
                        }

                        // 2. 插入历史记录
                        string insertHisSql = @"INSERT INTO eqp_group_his 
                                                (eqp_group_id, eqp_group_type, eqp_group_description, factory_id, 
                                                 event_user, event_remark, edit_time, event_type)
                                              VALUES 
                                                (@EqpGroupId, @EqpGroupType, @EqpGroupDescription, @FactoryId, 
                                                 @EventUser, @EventRemark, @EditTime, @EventType)";
                        using (SqlCommand insertHisCmd = new SqlCommand(insertHisSql, conn, transaction))
                        {
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupId", group.EqpGroupId);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupType", group.EqpGroupType);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupDescription", (object)group.EqpGroupDescription ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@FactoryId", (object)group.FactoryId ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@EventUser", eventUser);
                            insertHisCmd.Parameters.AddWithValue("@EventRemark", "设备组更新");
                            insertHisCmd.Parameters.AddWithValue("@EditTime", now); // 操作时间
                            insertHisCmd.Parameters.AddWithValue("@EventType", "Update");

                            insertHisCmd.ExecuteNonQuery(); // 历史记录插入不检查受影响行数，失败会抛异常
                        }

                        transaction.Commit(); // 提交事务
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // 发生异常时回滚
                        Console.WriteLine("Error updating EqpGroup: " + ex.Message);
                        // 记录日志 LogHelper.Log(...)
                        throw; // 重新抛出异常，让上层处理
                    }
                }
            }
        }

        /// <summary>
        /// 删除设备组，并记录历史
        /// </summary>
        public bool DeleteEqpGroup(string eqpGroupId, string eventUser)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // 1. 获取当前记录以供历史存档
                        EqpGroup currentGroup = null;
                        string selectSql = "SELECT * FROM eqp_group WHERE eqp_group_id = @EqpGroupId";
                        using (SqlCommand selectCmd = new SqlCommand(selectSql, conn, transaction))
                        {
                            selectCmd.Parameters.AddWithValue("@EqpGroupId", eqpGroupId);
                            using (SqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    currentGroup = new EqpGroup
                                    {
                                        EqpGroupId = reader["eqp_group_id"].ToString(),
                                        EqpGroupType = reader["eqp_group_type"].ToString(),
                                        EqpGroupDescription = reader.IsDBNull(reader.GetOrdinal("eqp_group_description")) ? null : reader["eqp_group_description"].ToString(),
                                        FactoryId = reader.IsDBNull(reader.GetOrdinal("factory_id")) ? null : reader["factory_id"].ToString(),
                                        CreateTime = Convert.ToDateTime(reader["create_time"])
                                    };
                                }
                                reader.Close(); // 必须先关闭 Reader 才能执行后续命令
                            }
                        }

                        if (currentGroup == null)
                        {
                             // 记录不存在，无需删除，也无需记录历史
                            transaction.Rollback();
                            return false;
                        }

                        // 2. 插入历史记录
                        string insertHisSql = @"INSERT INTO eqp_group_his 
                                                (eqp_group_id, eqp_group_type, eqp_group_description, factory_id, 
                                                 event_user, event_remark, edit_time, event_type)
                                              VALUES 
                                                (@EqpGroupId, @EqpGroupType, @EqpGroupDescription, @FactoryId, 
                                                 @EventUser, @EventRemark, @EditTime, @EventType)";
                        using (SqlCommand insertHisCmd = new SqlCommand(insertHisSql, conn, transaction))
                        {
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupId", currentGroup.EqpGroupId);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupType", currentGroup.EqpGroupType);
                            insertHisCmd.Parameters.AddWithValue("@EqpGroupDescription", (object)currentGroup.EqpGroupDescription ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@FactoryId", (object)currentGroup.FactoryId ?? DBNull.Value);
                            insertHisCmd.Parameters.AddWithValue("@EventUser", eventUser); // 操作用户
                            insertHisCmd.Parameters.AddWithValue("@EventRemark", "设备组删除"); // 备注
                            insertHisCmd.Parameters.AddWithValue("@EditTime", DateTime.Now); // 操作时间
                            insertHisCmd.Parameters.AddWithValue("@EventType", "Delete"); // 操作类型

                            insertHisCmd.ExecuteNonQuery();
                        }

                        // 3. 删除主表记录
                        string deleteSql = "DELETE FROM eqp_group WHERE eqp_group_id = @EqpGroupId";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteSql, conn, transaction))
                        {
                            deleteCmd.Parameters.AddWithValue("@EqpGroupId", eqpGroupId);
                            int rowsAffected = deleteCmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                transaction.Commit(); // 提交事务
                                return true;
                            }
                            else
                            {
                                transaction.Rollback(); // 如果删除失败则回滚
                                return false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback(); // 发生异常时回滚
                        Console.WriteLine("Error deleting EqpGroup: " + ex.Message);
                        // 可以选择记录日志 LogHelper.Log(...)
                        throw; // 重新抛出异常，让上层处理
                    }
                }
            }
        }
        
        /// <summary>
        /// 根据ID获取单个设备组信息 (用于编辑)
        /// </summary>
        public EqpGroup GetEqpGroupById(string eqpGroupId)
        {
             // TODO: 实现根据ID获取单个设备组的详细信息
             return null; // 占位符
        }

        /// <summary>
        /// 获取所有不同的设备组类型
        /// </summary>
        /// <returns>字符串列表，包含所有唯一的设备组类型</returns>
        public List<string> GetDistinctEqpGroupTypes()
        {
            List<string> types = new List<string>();
            string sql = "SELECT DISTINCT eqp_group_type FROM eqp_group WHERE eqp_group_type IS NOT NULL AND eqp_group_type <> '' ORDER BY eqp_group_type";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                types.Add(reader["eqp_group_type"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting distinct EqpGroup types: " + ex.Message);
                // 可以选择记录日志或抛出异常
                throw; 
            }
            return types;
        }

        /// <summary>
        /// 获取所有设备组的基础信息 (ID 和 Description) 用于下拉列表
        /// </summary>
        /// <returns>包含设备组基础信息的列表</returns>
        public List<EqpGroup> GetAllEqpGroupsBasicInfo()
        {
            List<EqpGroup> groups = new List<EqpGroup>();
            string sql = "SELECT eqp_group_id, eqp_group_description FROM eqp_group ORDER BY eqp_group_id";

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                groups.Add(new EqpGroup
                                {
                                    EqpGroupId = reader["eqp_group_id"].ToString(),
                                    // 处理 description 可能为 null 的情况
                                    EqpGroupDescription = reader.IsDBNull(reader.GetOrdinal("eqp_group_description")) ? string.Empty : reader["eqp_group_description"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting EqpGroup basic info: " + ex.Message);
                throw;
            }
            return groups;
        }
    }
} 