using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    /// <summary>
    /// 产品数据访问层
    /// </summary>
    public class ProductDAL
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 获取所有产品数据
        /// </summary>
        /// <returns>包含产品数据的DataTable</returns>
        public DataTable GetAllProducts()
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"SELECT p.ProductId, p.ProductName, p.CategoryId, pc.CategoryName,
                                p.Specification, p.Unit, p.Price, p.Cost, 
                                p.Description, p.Status, p.CreateTime
                                FROM Product p
                                LEFT JOIN ProductCategory pc ON p.CategoryId = pc.CategoryId
                                ORDER BY p.ProductId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    conn.Open();
                    adapter.Fill(dt);
                    Console.WriteLine($"从数据库加载了 {dt.Rows.Count} 条产品数据");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取产品数据失败: {ex.Message}");
                throw;
            }

            return dt;
        }

        /// <summary>
        /// 根据ID获取产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品对象</returns>
        public Product GetProductById(string productId)
        {
            Product product = null;

            try
            {
                string query = @"SELECT p.ProductId, p.ProductName, p.CategoryId, pc.CategoryName,
                                p.Specification, p.Unit, p.Price, p.Cost, 
                                p.Description, p.Status, p.CreateTime
                                FROM Product p
                                LEFT JOIN ProductCategory pc ON p.CategoryId = pc.CategoryId
                                WHERE p.ProductId = @ProductId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                ProductId = reader["ProductId"].ToString(),
                                ProductName = reader["ProductName"].ToString(),
                                CategoryId = reader["CategoryId"].ToString(),
                                Specification = reader["Specification"] != DBNull.Value ? reader["Specification"].ToString() : null,
                                Unit = reader["Unit"] != DBNull.Value ? reader["Unit"].ToString() : null,
                                Price = reader["Price"] != DBNull.Value ? (decimal?)reader["Price"] : null,
                                Cost = reader["Cost"] != DBNull.Value ? (decimal?)reader["Cost"] : null,
                                Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : null,
                                Status = reader["Status"].ToString(),
                                CreateTime = Convert.ToDateTime(reader["CreateTime"])
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"获取产品失败: {ex.Message}");
                throw;
            }

            return product;
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="categoryId">类别ID</param>
        /// <returns>符合搜索条件的产品数据</returns>
        public DataTable SearchProducts(string productName = null, string productCode = null, string categoryId = null)
        {
            DataTable dt = new DataTable();

            try
            {
                string query = @"SELECT p.ProductId, p.ProductName, p.CategoryId, pc.CategoryName,
                                p.Specification, p.Unit, p.Price, p.Cost, 
                                p.Description, p.Status, p.CreateTime
                                FROM Product p
                                LEFT JOIN ProductCategory pc ON p.CategoryId = pc.CategoryId
                                WHERE 1=1";

                if (!string.IsNullOrEmpty(productName))
                    query += " AND p.ProductName LIKE @ProductName";

                if (!string.IsNullOrEmpty(productCode))
                    query += " AND p.ProductId LIKE @ProductCode";

                if (!string.IsNullOrEmpty(categoryId))
                    query += " AND p.CategoryId = @CategoryId";

                query += " ORDER BY p.ProductId";

                using (SqlConnection conn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (!string.IsNullOrEmpty(productName))
                        cmd.Parameters.AddWithValue("@ProductName", "%" + productName + "%");

                    if (!string.IsNullOrEmpty(productCode))
                        cmd.Parameters.AddWithValue("@ProductCode", "%" + productCode + "%");

                    if (!string.IsNullOrEmpty(categoryId))
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        conn.Open();
                        adapter.Fill(dt);
                        Console.WriteLine($"搜索到 {dt.Rows.Count} 条产品数据");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"搜索产品失败: {ex.Message}");
                throw;
            }

            return dt;
        }
    }
} 