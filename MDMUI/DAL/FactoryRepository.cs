using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    /// <summary>
    /// 处理工厂数据访问
    /// </summary>
    public class FactoryRepository
    {
        private string connectionString = DbConnectionHelper.GetConnectionString();

        /// <summary>
        /// 获取所有工厂 (已废弃，使用 GetFiltered 替代)
        /// </summary>
        [Obsolete("Use GetFiltered instead to handle permissions and searching.")]
        public List<Factory> GetAll()
        {
            // 此方法不再推荐使用，因为未处理权限和搜索
            return GetFiltered(null, null);
        }

        /// <summary>
        /// 根据用户权限和搜索条件过滤工厂
        /// </summary>
        public List<Factory> GetFiltered(User user, string searchTerm = null)
        {
            List<Factory> factories = new List<Factory>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 添加 LEFT JOIN Employee 获取负责人姓名
                string query = @"SELECT 
                                    f.FactoryId, f.FactoryName, f.Address, f.Phone, 
                                    f.ManagerEmployeeId, e.EmployeeName as ManagerName 
                                FROM 
                                    Factory f
                                LEFT JOIN 
                                    Employee e ON f.ManagerEmployeeId = e.EmployeeId"; // JOIN Employee table
                List<string> conditions = new List<string>();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    conditions.Add("f.FactoryName LIKE @SearchTerm"); // Use alias
                    // 使用 N 前缀确保 Unicode 搜索正确 (参数化查询通常会处理好)
                    command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%"); 
                }

                if (user != null && user.RoleName != "超级管理员" && !string.IsNullOrEmpty(user.FactoryId))
                {
                    conditions.Add("f.FactoryId = @UserFactoryId"); // Use alias
                    command.Parameters.AddWithValue("@UserFactoryId", user.FactoryId);
                }

                if (conditions.Count > 0)
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }

                query += " ORDER BY f.FactoryId"; // Use alias
                command.CommandText = query;

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        factories.Add(MapReaderToFactory(reader));
                    }
                }
            }
            return factories;
        }

        /// <summary>
        /// 根据ID获取工厂
        /// </summary>
        public Factory GetById(string factoryId)
        {
            Factory factory = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                 // 添加 LEFT JOIN Employee 获取负责人姓名
                string query = @"SELECT 
                                    f.FactoryId, f.FactoryName, f.Address, f.Phone, 
                                    f.ManagerEmployeeId, e.EmployeeName as ManagerName 
                                FROM 
                                    Factory f
                                LEFT JOIN 
                                    Employee e ON f.ManagerEmployeeId = e.EmployeeId
                                WHERE f.FactoryId = @FactoryId"; // JOIN and use alias in WHERE
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FactoryId", factoryId);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        factory = MapReaderToFactory(reader);
                    }
                }
            }
            return factory;
        }

        /// <summary>
        /// 添加新工厂
        /// </summary>
        public bool Add(Factory factory)
        {
            if (factory == null || string.IsNullOrWhiteSpace(factory.FactoryId) || string.IsNullOrWhiteSpace(factory.FactoryName))
            {
                throw new ArgumentException("工厂对象无效或缺少必要的 ID 和名称。");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 检查 FactoryId 是否已存在
                string checkQuery = "SELECT COUNT(*) FROM Factory WHERE FactoryId = @FactoryId";
                SqlCommand checkCmd = new SqlCommand(checkQuery, connection);
                checkCmd.Parameters.AddWithValue("@FactoryId", factory.FactoryId);
                if ((int)checkCmd.ExecuteScalar() > 0)
                {
                    throw new InvalidOperationException($"工厂编号 '{factory.FactoryId}' 已存在。");
                }

                // 添加 ManagerEmployeeId 到 INSERT 语句
                string insertQuery = @"INSERT INTO Factory (FactoryId, FactoryName, Address, Phone, ManagerEmployeeId) 
                                     VALUES (@FactoryId, @FactoryName, @Address, @Phone, @ManagerEmployeeId)";
                SqlCommand command = new SqlCommand(insertQuery, connection);

                command.Parameters.AddWithValue("@FactoryId", factory.FactoryId);
                command.Parameters.AddWithValue("@FactoryName", (object)factory.FactoryName ?? DBNull.Value).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@Address", (object)factory.Address ?? DBNull.Value).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@Phone", (object)factory.Phone ?? DBNull.Value).SqlDbType = SqlDbType.VarChar;
                command.Parameters.AddWithValue("@ManagerEmployeeId", (object)factory.ManagerEmployeeId ?? DBNull.Value).SqlDbType = SqlDbType.VarChar; // 添加 ManagerEmployeeId 参数

                // 使用 N 前缀确保 Unicode 插入正确 (对于 NVarChar 类型)
                // SqlParameter 对于 AddWithValue 通常会推断类型，但显式指定并确保N前缀语义是最佳实践
                // 注意：对于 @FactoryName, @Address, @Manager，如果它们包含 Unicode，直接传递 .NET string 即可，
                // ADO.NET 会处理。主要是在 T-SQL 字面量中需要 N 前缀。
                // 为清晰起见，我们已指定 SqlDbType.NVarChar。

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// 更新工厂信息
        /// </summary>
        public bool Update(Factory factory)
        {
             if (factory == null || string.IsNullOrWhiteSpace(factory.FactoryId) || string.IsNullOrWhiteSpace(factory.FactoryName))
            {
                throw new ArgumentException("工厂对象无效或缺少必要的 ID 和名称。");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 添加 ManagerEmployeeId 到 UPDATE 语句
                string updateQuery = @"UPDATE Factory SET 
                                        FactoryName = @FactoryName, 
                                        Address = @Address, 
                                        Phone = @Phone, 
                                        ManagerEmployeeId = @ManagerEmployeeId 
                                     WHERE FactoryId = @FactoryId";
                SqlCommand command = new SqlCommand(updateQuery, connection);

                command.Parameters.AddWithValue("@FactoryName", (object)factory.FactoryName ?? DBNull.Value).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@Address", (object)factory.Address ?? DBNull.Value).SqlDbType = SqlDbType.NVarChar;
                command.Parameters.AddWithValue("@Phone", (object)factory.Phone ?? DBNull.Value).SqlDbType = SqlDbType.VarChar;
                command.Parameters.AddWithValue("@ManagerEmployeeId", (object)factory.ManagerEmployeeId ?? DBNull.Value).SqlDbType = SqlDbType.VarChar; // 添加 ManagerEmployeeId 参数
                command.Parameters.AddWithValue("@FactoryId", factory.FactoryId); // WHERE 条件

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// 删除工厂 (关联检查已移至 Service 层)
        /// </summary>
        public bool Delete(string factoryId)
        {
             if (string.IsNullOrWhiteSpace(factoryId))
            {
                throw new ArgumentException("工厂 ID 不能为空。");
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string deleteQuery = "DELETE FROM Factory WHERE FactoryId = @FactoryId";
                SqlCommand command = new SqlCommand(deleteQuery, connection);
                command.Parameters.AddWithValue("@FactoryId", factoryId);

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// 检查工厂是否关联了用户或部门
        /// </summary>
        public bool IsFactoryAssociated(string factoryId)
        {
            if (string.IsNullOrWhiteSpace(factoryId))
            {
                throw new ArgumentException("工厂 ID 不能为空。");
            }

             using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // 检查 UserFactory 表 (而不是 Users 表)
                string checkUsersQuery = "SELECT COUNT(*) FROM UserFactory WHERE FactoryId = @FactoryId";
                SqlCommand userCmd = new SqlCommand(checkUsersQuery, connection);
                userCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                if ((int)userCmd.ExecuteScalar() > 0)
                {
                    return true;
                }

                // 检查 Department 表 (这个查询是正确的)
                string checkDeptsQuery = "SELECT COUNT(*) FROM Department WHERE FactoryId = @FactoryId";
                 SqlCommand deptCmd = new SqlCommand(checkDeptsQuery, connection);
                deptCmd.Parameters.AddWithValue("@FactoryId", factoryId);
                if ((int)deptCmd.ExecuteScalar() > 0)
                {
                    return true;
                }

                // TODO: 如果还有其他表关联 Factory，也需要在这里检查
            }
             return false; // 没有找到关联
        }

        // 辅助方法：将 SqlDataReader 映射到 Factory 对象
        private Factory MapReaderToFactory(SqlDataReader reader)
        {
            return new Factory
            {
                FactoryId = reader["FactoryId"].ToString(),
                FactoryName = reader["FactoryName"] == DBNull.Value ? null : reader["FactoryName"].ToString(),
                Address = reader["Address"] == DBNull.Value ? null : reader["Address"].ToString(),
                Phone = reader["Phone"] == DBNull.Value ? null : reader["Phone"].ToString(),
                ManagerEmployeeId = reader["ManagerEmployeeId"] == DBNull.Value ? null : reader["ManagerEmployeeId"].ToString(), // 添加 ManagerEmployeeId 映射
                ManagerName = reader["ManagerName"] == DBNull.Value ? null : reader["ManagerName"].ToString() // 添加 ManagerName 映射 (来自 JOIN)
            };
        }
    }
} 