using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;
using System.Configuration; // 添加 ConfigurationManager 支持

namespace MDMUI.DAL
{
    public class FactoryDAL
    {
        // 从 App.config 读取连接字符串
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Factory> GetAllFactories()
        {
            List<Factory> factories = new List<Factory>();
            // 查询时 JOIN Employee 表获取负责人姓名
            string query = @"SELECT f.FactoryId, f.FactoryName, f.Address, f.Phone, f.ManagerEmployeeId, e.EmployeeName as ManagerName
                           FROM Factory f
                           LEFT JOIN Employee e ON f.ManagerEmployeeId = e.EmployeeId";

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
                            factories.Add(MapReaderToFactory(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取所有工厂信息失败: " + ex.Message, ex);
                }
            }
            return factories;
        }

        public Factory GetFactoryById(string factoryId)
        {
             Factory factory = null;
             string query = @"SELECT f.FactoryId, f.FactoryName, f.Address, f.Phone, f.ManagerEmployeeId, e.EmployeeName as ManagerName
                           FROM Factory f
                           LEFT JOIN Employee e ON f.ManagerEmployeeId = e.EmployeeId
                           WHERE f.FactoryId = @FactoryId";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@FactoryId", factoryId);
                 try
                 {
                     connection.Open();
                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         if (reader.Read())
                         {
                             factory = MapReaderToFactory(reader);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("获取工厂信息失败: " + ex.Message, ex);
                 }
             }
             return factory;
        }

         public bool AddFactory(Factory factory)
         {
             string query = @"INSERT INTO Factory (FactoryId, FactoryName, Address, Phone, ManagerEmployeeId)
                            VALUES (@FactoryId, @FactoryName, @Address, @Phone, @ManagerEmployeeId)";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@FactoryId", factory.FactoryId);
                 command.Parameters.AddWithValue("@FactoryName", factory.FactoryName);
                 command.Parameters.AddWithValue("@Address", (object)factory.Address ?? DBNull.Value);
                 command.Parameters.AddWithValue("@Phone", (object)factory.Phone ?? DBNull.Value);
                 command.Parameters.AddWithValue("@ManagerEmployeeId", (object)factory.ManagerEmployeeId ?? DBNull.Value);

                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                  catch (SqlException ex) when (ex.Number == 2627) // PK violation
                 {
                     throw new Exception($"添加工厂失败：工厂ID '{factory.FactoryId}' 已存在。", ex);
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("添加工厂失败: " + ex.Message, ex);
                 }
             }
         }

        public bool UpdateFactory(Factory factory)
        {
             string query = @"UPDATE Factory SET
                                FactoryName = @FactoryName,
                                Address = @Address,
                                Phone = @Phone,
                                ManagerEmployeeId = @ManagerEmployeeId
                            WHERE FactoryId = @FactoryId";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@FactoryName", factory.FactoryName);
                 command.Parameters.AddWithValue("@Address", (object)factory.Address ?? DBNull.Value);
                 command.Parameters.AddWithValue("@Phone", (object)factory.Phone ?? DBNull.Value);
                 command.Parameters.AddWithValue("@ManagerEmployeeId", (object)factory.ManagerEmployeeId ?? DBNull.Value);
                 command.Parameters.AddWithValue("@FactoryId", factory.FactoryId);

                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("更新工厂信息失败: " + ex.Message, ex);
                 }
             }
        }

        public bool DeleteFactory(string factoryId)
        {
            // 注意：删除工厂前可能需要检查是否有部门、用户等关联数据
             string query = "DELETE FROM Factory WHERE FactoryId = @FactoryId";
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@FactoryId", factoryId);
                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                  catch (SqlException ex) when (ex.Number == 547) // FK constraint violation
                 {
                     throw new Exception("删除工厂失败：该工厂下可能存在关联的部门或其他数据。", ex);
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("删除工厂失败: " + ex.Message, ex);
                 }
             }
        }

        private Factory MapReaderToFactory(SqlDataReader reader)
        {
            return new Factory
            {
                FactoryId = reader["FactoryId"].ToString(),
                FactoryName = reader["FactoryName"].ToString(),
                Address = reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader["Address"].ToString(),
                Phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? null : reader["Phone"].ToString(),
                ManagerEmployeeId = reader.IsDBNull(reader.GetOrdinal("ManagerEmployeeId")) ? null : reader["ManagerEmployeeId"].ToString(),
                ManagerName = reader.IsDBNull(reader.GetOrdinal("ManagerName")) ? null : reader["ManagerName"].ToString()
            };
        }
    }
} 