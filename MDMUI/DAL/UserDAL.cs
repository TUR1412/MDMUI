using System;
using System.Collections.Generic; // 添加 List 支持
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;


namespace MDMUI.DAL
{
    public class UserDAL
    {
        // 从 App.config 读取连接字符串
        private string connectionString = DbConnectionHelper.GetConnectionString();

        // 验证用户登录
        public User ValidateUser(string username, string password)
        {
            User user = null;
            // 查询用户基本信息和角色名
            string userQuery = @"SELECT u.Id, u.Username, u.Password, u.RealName, u.RoleId, r.RoleName, u.LastLoginTime
                               FROM Users u
                               LEFT JOIN Roles r ON u.RoleId = r.RoleId
                               WHERE u.Username = @Username";
            // 查询默认工厂ID
            string factoryQuery = "SELECT FactoryId FROM UserFactory WHERE UserId = @UserId AND IsDefault = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // 1. 获取用户基本信息
                    using (SqlCommand userCommand = new SqlCommand(userQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = userCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedPassword = reader["Password"].ToString();
                                if (PasswordEncryptor.VerifyPassword(password, storedPassword))
                                {
                                    user = MapReaderToUser(reader); // 先映射基本信息
                                }
                            }
                        } // reader disposed here
                    }

                    // 2. 如果用户验证成功，获取默认工厂ID
                    if (user != null)
                    {
                        using (SqlCommand factoryCommand = new SqlCommand(factoryQuery, connection))
                        {
                            factoryCommand.Parameters.AddWithValue("@UserId", user.Id);
                            object result = factoryCommand.ExecuteScalar(); // 可能返回 null
                            if (result != null && result != DBNull.Value)
                            {
                                user.FactoryId = result.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("数据库操作失败: " + ex.Message, ex);
                }
            }
            return user;
        }

        // 更新最后登录时间
        public bool UpdateLastLoginTime(int userId)
        {
            string query = "UPDATE Users SET LastLoginTime = @LastLoginTime WHERE Id = @Id";
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@LastLoginTime", DateTime.Now);
                command.Parameters.AddWithValue("@Id", userId);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                     // Consider logging the exception details
                    throw new Exception("更新登录时间失败: " + ex.Message, ex);
                }
            }
        }

        // 获取所有用户信息 (通常列表不需要默认工厂，保持不变或按需修改)
        public List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            // 注意：此查询不包含 FactoryId，因为列表展示通常不需要默认工厂
            // 如果确实需要，需要为每个用户单独查询或修改查询逻辑 (可能影响性能)
            string query = @"SELECT u.Id, u.Username, u.Password, u.RealName, u.RoleId, r.RoleName, u.LastLoginTime
                           FROM Users u
                           LEFT JOIN Roles r ON u.RoleId = r.RoleId";

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
                            users.Add(MapReaderToUser(reader)); // MapReaderToUser 现在不包含 FactoryId
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Consider logging the exception details
                    throw new Exception("获取所有用户信息失败: " + ex.Message, ex);
                }
            }
            return users;
        }

        // 获取单个用户信息 (添加获取默认工厂逻辑)
        public User GetUserById(int userId)
        {
            User user = null;
            string userQuery = @"SELECT u.Id, u.Username, u.Password, u.RealName, u.RoleId, r.RoleName, u.LastLoginTime
                           FROM Users u
                           LEFT JOIN Roles r ON u.RoleId = r.RoleId
                           WHERE u.Id = @Id";
            string factoryQuery = "SELECT FactoryId FROM UserFactory WHERE UserId = @UserId AND IsDefault = 1";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                 try
                 {
                     connection.Open();
                     // 1. 获取用户基本信息
                     using (SqlCommand userCommand = new SqlCommand(userQuery, connection))
                     {
                         userCommand.Parameters.AddWithValue("@Id", userId);
                         using (SqlDataReader reader = userCommand.ExecuteReader())
                         {
                             if (reader.Read())
                             {
                                 user = MapReaderToUser(reader);
                             }
                         }
                     }

                     // 2. 如果用户存在，获取默认工厂ID
                     if (user != null)
                     {
                         using (SqlCommand factoryCommand = new SqlCommand(factoryQuery, connection))
                         {
                            factoryCommand.Parameters.AddWithValue("@UserId", user.Id);
                             object result = factoryCommand.ExecuteScalar();
                             if (result != null && result != DBNull.Value)
                             {
                                 user.FactoryId = result.ToString();
                             }
                         }
                     }
                 }
                catch (Exception ex)
                {
                    throw new Exception("获取用户信息失败: " + ex.Message, ex);
                }
            }
            return user;
        }

        // 根据用户名获取用户信息（不校验密码）
        public User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            User user = null;
            string userQuery = @"SELECT u.Id, u.Username, u.Password, u.RealName, u.RoleId, r.RoleName, u.LastLoginTime
                           FROM Users u
                           LEFT JOIN Roles r ON u.RoleId = r.RoleId
                           WHERE u.Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand userCommand = new SqlCommand(userQuery, connection))
                    {
                        userCommand.Parameters.AddWithValue("@Username", username);
                        using (SqlDataReader reader = userCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                user = MapReaderToUser(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取用户信息失败: " + ex.Message, ex);
                }
            }

            return user;
        }

        // 添加新用户
        public bool AddUser(User user)
        {
            // 注意：密码应该在调用此方法之前就已经加密
            string query = @"INSERT INTO Users (Username, Password, RealName, RoleId)
                           VALUES (@Username, @Password, @RealName, @RoleId)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@Password", user.Password); // 假设传入的是已加密的密码
                command.Parameters.AddWithValue("@RealName", (object)user.RealName ?? DBNull.Value);
                command.Parameters.AddWithValue("@RoleId", (object)user.RoleId ?? DBNull.Value);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (SqlException ex) when (ex.Number == 2627) // Unique constraint violation
                {
                     throw new Exception($"添加用户失败：用户名 '{user.Username}' 已存在。", ex);
                }
                catch (Exception ex)
                {
                     // Consider logging the exception details
                    throw new Exception("添加用户失败: " + ex.Message, ex);
                }
            }
        }

        // 更新用户信息 (不包括密码)
        public bool UpdateUser(User user)
        {
            string query = @"UPDATE Users SET
                               RealName = @RealName,
                               RoleId = @RoleId
                           WHERE Id = @Id";
            // 如果需要同时更新用户名，请确保处理唯一性约束
            // string query = "UPDATE Users SET Username = @Username, RealName = @RealName, RoleId = @RoleId WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                // command.Parameters.AddWithValue("@Username", user.Username);
                command.Parameters.AddWithValue("@RealName", (object)user.RealName ?? DBNull.Value);
                command.Parameters.AddWithValue("@RoleId", (object)user.RoleId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Id", user.Id);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                    // Consider logging the exception details
                    throw new Exception("更新用户信息失败: " + ex.Message, ex);
                }
            }
        }

        // 删除用户
         public bool DeleteUser(int userId)
         {
             // 注意：可能需要考虑与该用户相关的其他数据（如日志、操作记录等）的处理策略
             string query = "DELETE FROM Users WHERE Id = @Id";
             // 防止删除 admin 用户
             // string query = "DELETE FROM Users WHERE Id = @Id AND Username <> 'admin'";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@Id", userId);

                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                 catch (Exception ex)
                 {
                     // Consider logging the exception details
                    throw new Exception("删除用户失败: " + ex.Message, ex);
                 }
             }
         }


        // 修改用户密码
        public bool ChangePassword(int userId, string newPassword)
        {
            string query = "UPDATE Users SET Password = @Password WHERE Id = @Id";
            string encryptedPassword = PasswordEncryptor.EncryptPassword(newPassword); // 加密

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Password", encryptedPassword);
                command.Parameters.AddWithValue("@Id", userId);

                try
                {
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
                catch (Exception ex)
                {
                     // Consider logging the exception details
                    throw new Exception("修改密码失败: " + ex.Message, ex);
                }
            }
        }

        // 重置用户密码 (例如，重置为 '123456')
        public bool ResetPassword(int userId, string defaultPassword = "123456")
        {
             // ChangePassword 内部会负责加密，这里必须传入明文，避免二次加密导致无法登录。
             return ChangePassword(userId, defaultPassword);
        }


        // 辅助方法：将 SqlDataReader 的当前行映射到 User 对象 (移除 FactoryId 的直接映射)
        private User MapReaderToUser(SqlDataReader reader)
        {
            return new User
            {
                Id = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                RealName = reader.IsDBNull(reader.GetOrdinal("RealName")) ? null : reader["RealName"].ToString(),
                RoleId = reader.IsDBNull(reader.GetOrdinal("RoleId")) ? (int?)null : Convert.ToInt32(reader["RoleId"]),
                RoleName = reader.IsDBNull(reader.GetOrdinal("RoleName")) ? null : reader["RoleName"].ToString(),
                LastLoginTime = reader.IsDBNull(reader.GetOrdinal("LastLoginTime")) ? (DateTime?)null : Convert.ToDateTime(reader["LastLoginTime"]),
                // FactoryId 会在调用方法中单独查询填充，这里不再映射
                // FactoryId = reader.IsDBNull(reader.GetOrdinal("FactoryId")) ? null : reader["FactoryId"].ToString() // 移除
            };
        }


        /*
        // 确保Users表存在，并包含所需的列 (此方法与 dbo.sql 管理方式冲突，建议移除或重构为检查版本)
        private void EnsureUsersTableExists(SqlConnection connection)
        {
            // ... (原有代码) ...
            // 此处逻辑复杂且与当前数据库结构不符，暂时注释掉
        }
        */
    }
}
