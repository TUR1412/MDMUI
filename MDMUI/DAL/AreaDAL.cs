using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MDMUI.Model;

namespace MDMUI.DAL
{
    /// <summary>
    /// 数据访问类 - 区域管理
    /// </summary>
    public class AreaDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 获取所有区域信息
        /// </summary>
        /// <returns>区域列表</returns>
        public List<Area> GetAllAreas()
        {
            List<Area> areas = new List<Area>();
            string query = "SELECT AreaId, AreaName, ParentAreaId, PostalCode, Remark FROM Area ORDER BY AreaName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            areas.Add(MapReaderToArea(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    // 实际项目中应记录日志
                    Console.WriteLine($"Error getting all areas: {ex.Message}");
                    throw new Exception("获取所有区域信息失败。", ex);
                }
            }
            return areas;
        }

        /// <summary>
        /// 根据ID获取单个区域信息
        /// </summary>
        /// <param name="areaId">区域ID</param>
        /// <returns>区域对象，如果找不到则返回null</returns>
        public Area GetAreaById(string areaId)
        {
            Area area = null;
            string query = "SELECT AreaId, AreaName, ParentAreaId, PostalCode, Remark FROM Area WHERE AreaId = @AreaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AreaId", areaId);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            area = MapReaderToArea(reader);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting area by ID {areaId}: {ex.Message}");
                    throw new Exception("获取区域信息失败。", ex);
                }
            }
            return area;
        }

        /// <summary>
        /// 添加新区域
        /// </summary>
        /// <param name="area">要添加的区域对象</param>
        /// <returns>是否添加成功</returns>
        public bool AddArea(Area area)
        {
            string query = @"INSERT INTO Area (AreaId, AreaName, ParentAreaId, PostalCode, Remark)
                           VALUES (@AreaId, @AreaName, @ParentAreaId, @PostalCode, @Remark)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AreaId", area.AreaId);
                // 使用 SqlDbType.NVarChar 处理可能包含非 ASCII 字符的字段
                command.Parameters.Add("@AreaName", SqlDbType.NVarChar).Value = area.AreaName;
                command.Parameters.AddWithValue("@ParentAreaId", (object)area.ParentAreaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@PostalCode", (object)area.PostalCode ?? DBNull.Value);
                command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = (object)area.Remark ?? DBNull.Value;

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex) when (ex.Number == 2627) // 主键冲突
                {
                    Console.WriteLine($"Error adding area: Primary key violation for ID {area.AreaId}");
                    throw new Exception($"添加区域失败：区域ID '{area.AreaId}' 已存在。", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error adding area: {ex.Message}");
                    throw new Exception("添加区域失败。", ex);
                }
            }
        }

        /// <summary>
        /// 更新区域信息
        /// </summary>
        /// <param name="area">要更新的区域对象</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateArea(Area area)
        {
            string query = @"UPDATE Area SET
                               AreaName = @AreaName,
                               ParentAreaId = @ParentAreaId,
                               PostalCode = @PostalCode,
                               Remark = @Remark
                           WHERE AreaId = @AreaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // 使用 SqlDbType.NVarChar 处理可能包含非 ASCII 字符的字段
                command.Parameters.Add("@AreaName", SqlDbType.NVarChar).Value = area.AreaName;
                command.Parameters.AddWithValue("@ParentAreaId", (object)area.ParentAreaId ?? DBNull.Value);
                command.Parameters.AddWithValue("@PostalCode", (object)area.PostalCode ?? DBNull.Value);
                command.Parameters.Add("@Remark", SqlDbType.NVarChar).Value = (object)area.Remark ?? DBNull.Value;
                command.Parameters.AddWithValue("@AreaId", area.AreaId);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    // 即使没有行受影响（例如数据未更改），也可能不抛异常
                    // 根据需求，可以检查 result 是否 > 0，或允许 0 表示"未找到或未更改"
                    return result >= 0; // 认为未找到或未更改不算失败
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error updating area ID {area.AreaId}: {ex.Message}");
                    throw new Exception("更新区域信息失败。", ex);
                }
            }
        }

        /// <summary>
        /// 删除区域
        /// </summary>
        /// <param name="areaId">要删除的区域ID</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteArea(string areaId)
        {
            // 警告: 此处未检查子区域。如果需要，应在 BLL 层实现检查逻辑。
            // 如果数据库设置了级联删除或外键约束，行为会不同。
            string query = "DELETE FROM Area WHERE AreaId = @AreaId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@AreaId", areaId);
                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex) when (ex.Number == 547) // 外键约束冲突
                {
                     Console.WriteLine($"Error deleting area ID {areaId}: Foreign key constraint.");
                     throw new Exception("删除区域失败：该区域可能被其他数据引用（如作为子区域的上级）。", ex);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting area ID {areaId}: {ex.Message}");
                    throw new Exception("删除区域失败。", ex);
                }
            }
        }
        
        /// <summary>
        /// 检查是否存在指定父区域的子区域
        /// </summary>
        /// <param name="parentAreaId">父区域ID</param>
        /// <returns>是否存在子区域</returns>
        public bool HasChildAreas(string parentAreaId)
        {
             string query = "SELECT COUNT(*) FROM Area WHERE ParentAreaId = @ParentAreaId";
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@ParentAreaId", parentAreaId);
                 try
                 {
                     connection.Open();
                     int count = (int)command.ExecuteScalar();
                     return count > 0;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error checking child areas for parent {parentAreaId}: {ex.Message}");
                     // 发生错误时，保守起见，假定存在子区域以阻止删除
                     return true; 
                 }
             }
        }


        /// <summary>
        /// 辅助方法：将 SqlDataReader 的当前行映射到 Area 对象
        /// </summary>
        private Area MapReaderToArea(SqlDataReader reader)
        {
            return new Area
            {
                AreaId = reader["AreaId"].ToString(),
                AreaName = reader["AreaName"].ToString(),
                ParentAreaId = reader.IsDBNull(reader.GetOrdinal("ParentAreaId")) ? null : reader["ParentAreaId"].ToString(),
                PostalCode = reader.IsDBNull(reader.GetOrdinal("PostalCode")) ? null : reader["PostalCode"].ToString(),
                Remark = reader.IsDBNull(reader.GetOrdinal("Remark")) ? null : reader["Remark"].ToString()
            };
        }
    }
} 