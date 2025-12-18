using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MDMUI.DAL
{
    public class ProcessDAL
    {
        private readonly string connectionString;

        public ProcessDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// 根据工艺包ID获取工艺流程
        /// </summary>
        /// <param name="packageId">工艺包ID</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetProcessesByPackageId(string packageId)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT ProcessId, Version, PackageId, 
                               Description, ProductionType, 
                               Sequence, IsCurrentlyUsed
                        FROM Process
                        WHERE PackageId = @PackageId
                        ORDER BY Sequence";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PackageId", packageId);
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"根据工艺包ID获取工艺流程数据失败: {ex.Message}");
                throw;
            }
            
            return dt;
        }
    }
} 