using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MDMUI.DAL
{
    public class ProcessPackageDAL
    {
        private readonly string connectionString;

        public ProcessPackageDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// 获取所有工艺包
        /// </summary>
        /// <returns>工艺包数据表</returns>
        public DataTable GetAllProcessPackages()
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.PackageId, p.Version, 
                               CAST(p.Description AS NVARCHAR(255)) AS Description, 
                               p.ProductId, 
                               CAST(prod.ProductName AS NVARCHAR(255)) AS ProductName, 
                               p.CreateTime, 
                               CAST(p.Status AS NVARCHAR(50)) AS Status
                        FROM ProcessPackage p
                        LEFT JOIN Product prod ON p.ProductId = prod.ProductId
                        ORDER BY p.PackageId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
                Console.WriteLine($"获取工艺包数据失败: {ex.Message}");
                throw;
            }
            
            return dt;
        }

        /// <summary>
        /// 根据产品ID获取工艺包
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺包数据表</returns>
        public DataTable GetProcessPackagesByProductId(string productId)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.PackageId, p.Version, 
                               CAST(p.Description AS NVARCHAR(255)) AS Description, 
                               p.ProductId, 
                               CAST(prod.ProductName AS NVARCHAR(255)) AS ProductName, 
                               p.CreateTime, 
                               CAST(p.Status AS NVARCHAR(50)) AS Status
                        FROM ProcessPackage p
                        LEFT JOIN Product prod ON p.ProductId = prod.ProductId
                        WHERE p.ProductId = @ProductId
                        ORDER BY p.PackageId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProductId", productId);
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
                Console.WriteLine($"根据产品ID获取工艺包数据失败: {ex.Message}");
                throw;
            }
            
            return dt;
        }
    }
} 