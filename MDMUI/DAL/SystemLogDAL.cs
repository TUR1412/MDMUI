using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    /// <summary>
    /// 系统日志数据访问类
    /// </summary>
    public class SystemLogDAL
    {
        private string connectionString = DbConnectionHelper.GetConnectionString();

        /// <summary>
        /// 获取系统日志记录 (支持过滤)
        /// </summary>
        /// <param name="startDate">开始日期 (可选)</param>
        /// <param name="endDate">结束日期 (可选)</param>
        /// <param name="userId">用户ID (可选, 0 表示不筛选)</param>
        /// <param name="operationType">操作类型 (可选, null或空表示不筛选)</param>
        /// <param name="operationModule">操作模块 (可选, null或空表示不筛选)</param>
        /// <returns>包含日志信息的DataTable</returns>
        public DataTable GetSystemLogs(DateTime? startDate, DateTime? endDate, int? userId, string operationType, string operationModule)
        {
            DataTable dt = new DataTable();
            List<SqlParameter> parameters = new List<SqlParameter>();
            string sql = @"SELECT 
                            LogId, 
                            UserId, 
                            UserName, 
                            OperationType, 
                            OperationModule, 
                            Description, 
                            IPAddress, 
                            LogTime 
                          FROM SystemLog 
                          WHERE 1=1";

            if (startDate.HasValue)
            {
                sql += " AND LogTime >= @StartDate";
                // 修复：确保使用日期的开始时间（00:00:00）
                parameters.Add(new SqlParameter("@StartDate", startDate.Value.Date)); 
            }

            if (endDate.HasValue)
            {
                sql += " AND LogTime <= @EndDate";
                // 修复：确保使用日期的结束时间（23:59:59.999）
                parameters.Add(new SqlParameter("@EndDate", endDate.Value.Date.AddDays(1).AddSeconds(-1))); 
            }

            if (userId.HasValue && userId.Value > 0) // Assuming 0 or null means all users
            {
                sql += " AND UserId = @UserId";
                parameters.Add(new SqlParameter("@UserId", userId.Value));
            }

            if (!string.IsNullOrEmpty(operationType))
            {
                sql += " AND OperationType LIKE @OperationType";
                parameters.Add(new SqlParameter("@OperationType", "%" + operationType + "%"));
            }

            if (!string.IsNullOrEmpty(operationModule))
            {
                sql += " AND OperationModule LIKE @OperationModule";
                parameters.Add(new SqlParameter("@OperationModule", "%" + operationModule + "%"));
            }

            sql += " ORDER BY LogTime DESC"; // 按时间降序排序

            try
            {
                 using (SqlConnection conn = new SqlConnection(connectionString))
                 {
                     conn.Open();
                     Console.WriteLine("数据库连接已打开，开始执行查询");
                     using (SqlCommand cmd = new SqlCommand(sql, conn))
                     {
                         if (parameters.Count > 0)
                         {
                             cmd.Parameters.AddRange(parameters.ToArray());
                         }
                         using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                         {
                             adapter.Fill(dt);
                             Console.WriteLine($"查询成功，返回 {dt.Rows.Count} 条记录");
                         }
                     }
                 }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting SystemLog: " + ex.Message);
                // Log the error: LogHelper.Error("Error getting SystemLog", ex);
                throw; // Rethrow to allow upper layers to handle
            }

            return dt;
        }

        /// <summary>
        /// 获取日志中所有不同的用户名和ID
        /// </summary>
        /// <returns>字典，键是用户ID，值是用户名</returns>
        public Dictionary<int, string> GetDistinctLogUsers()
        {
            var users = new Dictionary<int, string>();
            string sql = "SELECT DISTINCT UserId, UserName FROM SystemLog ORDER BY UserName";

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
                                // Handle potential DBNull for UserId if necessary, though unlikely
                                int userId = Convert.ToInt32(reader["UserId"]);
                                string userName = reader["UserName"]?.ToString() ?? $"User ID {userId}"; 
                                if (!users.ContainsKey(userId)) // Ensure uniqueness in dictionary
                                {
                                    users.Add(userId, userName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting distinct log users: " + ex.Message);
                // LogHelper.Error("Error getting distinct log users", ex);
                throw;
            }
            return users;
        }

        /// <summary>
        /// 获取日志中所有不同的操作模块
        /// </summary>
        /// <returns>字符串列表</returns>
        public List<string> GetDistinctLogModules()
        {
            var modules = new List<string>();
            string sql = "SELECT DISTINCT OperationModule FROM SystemLog WHERE OperationModule IS NOT NULL AND OperationModule <> '' ORDER BY OperationModule";

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
                                modules.Add(reader["OperationModule"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting distinct log modules: " + ex.Message);
                // LogHelper.Error("Error getting distinct log modules", ex);
                throw;
            }
            return modules;
        }
        
        /// <summary>
        /// 添加一条系统日志记录
        /// </summary>
        public void AddLog(int userId, string userName, string operationType, string operationModule, string description, string ipAddress)
        {
            string sql = @"INSERT INTO SystemLog 
                            (UserId, UserName, OperationType, OperationModule, Description, IPAddress, LogTime)
                           VALUES 
                            (@UserId, @UserName, @OperationType, @OperationModule, @Description, @IPAddress, GETDATE())";

            SqlParameter[] parameters = {
                new SqlParameter("@UserId", userId),
                new SqlParameter("@UserName", userName ?? string.Empty), // Handle potential null username
                new SqlParameter("@OperationType", operationType ?? string.Empty),
                new SqlParameter("@OperationModule", operationModule ?? string.Empty),
                new SqlParameter("@Description", (object)description ?? DBNull.Value), // Handle null description
                new SqlParameter("@IPAddress", (object)ipAddress ?? DBNull.Value) // Handle null IP
            };

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddRange(parameters);
                        cmd.ExecuteNonQuery(); // Execute insert
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error adding SystemLog: " + ex.Message);
                // Log the error specifically for AddLog failure
                // LogHelper.Error("Error adding SystemLog", ex);
                // Depending on requirements, maybe rethrow or just log
            }
        }
    }
} 
