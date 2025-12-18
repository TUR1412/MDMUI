using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;
using System.Configuration; // 添加 ConfigurationManager 支持

namespace MDMUI.DAL
{
    public class DepartmentDAL
    {
        // 改为读取 DefaultConnection
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public List<Department> GetAllDepartments()
        {
            List<Department> departments = new List<Department>();
            string query = @"SELECT d.DeptId, d.DeptName, d.ParentDeptId, d.FactoryId, d.Description, d.CreateTime,
                                  d.ManagerEmployeeId, e.EmployeeName as ManagerName
                           FROM Department d
                           LEFT JOIN Employee e ON d.ManagerEmployeeId = e.EmployeeId";
                           // 可以添加 ORDER BY

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
                            departments.Add(MapReaderToDepartment(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("获取所有部门信息失败: " + ex.Message, ex);
                }
            }
            return departments;
        }
        
        // 添加根据工厂ID获取部门的方法
        public List<Department> GetDepartmentsByFactoryId(string factoryId)
        {
            List<Department> departments = new List<Department>();
            string query = @"SELECT d.DeptId, d.DeptName, d.ParentDeptId, d.FactoryId, d.Description, d.CreateTime,
                                  d.ManagerEmployeeId, e.EmployeeName as ManagerName
                           FROM Department d
                           LEFT JOIN Employee e ON d.ManagerEmployeeId = e.EmployeeId
                           WHERE d.FactoryId = @FactoryId ORDER BY d.DeptName"; 

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FactoryId", factoryId);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(MapReaderToDepartment(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"获取工厂 {factoryId} 的部门信息失败: " + ex.Message, ex);
                }
            }
            return departments;
        }

        public Department GetDepartmentById(string deptId)
        {
             Department department = null;
             string query = @"SELECT d.DeptId, d.DeptName, d.ParentDeptId, d.FactoryId, d.Description, d.CreateTime,
                                  d.ManagerEmployeeId, e.EmployeeName as ManagerName
                           FROM Department d
                           LEFT JOIN Employee e ON d.ManagerEmployeeId = e.EmployeeId
                           WHERE d.DeptId = @DeptId";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@DeptId", deptId);
                 try
                 {
                     connection.Open();
                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         if (reader.Read())
                         {
                             department = MapReaderToDepartment(reader);
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("获取部门信息失败: " + ex.Message, ex);
                 }
             }
             return department;
        }

        public bool AddDepartment(Department department)
        {
             string query = @"INSERT INTO Department (DeptId, DeptName, ParentDeptId, FactoryId, Description, ManagerEmployeeId, CreateTime)
                            VALUES (@DeptId, @DeptName, @ParentDeptId, @FactoryId, @Description, @ManagerEmployeeId, @CreateTime)";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@DeptId", department.DeptId);
                 // 显式指定 NVarChar
                 command.Parameters.Add("@DeptName", SqlDbType.NVarChar).Value = department.DeptName;
                 command.Parameters.AddWithValue("@ParentDeptId", (object)department.ParentDeptId ?? DBNull.Value);
                 command.Parameters.AddWithValue("@FactoryId", department.FactoryId);
                 // 显式指定 NVarChar
                 command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = (object)department.Description ?? DBNull.Value;
                 command.Parameters.AddWithValue("@ManagerEmployeeId", (object)department.ManagerEmployeeId ?? DBNull.Value);
                 command.Parameters.AddWithValue("@CreateTime", department.CreateTime == DateTime.MinValue ? DateTime.Now : department.CreateTime); // 提供默认创建时间

                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                  catch (SqlException ex) when (ex.Number == 2627) // PK violation
                 {
                     throw new Exception($"添加部门失败：部门ID '{department.DeptId}' 已存在。", ex);
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("添加部门失败: " + ex.Message, ex);
                 }
             }
        }

        public bool UpdateDepartment(Department department)
        {
             string query = @"UPDATE Department SET
                                DeptName = @DeptName,
                                ParentDeptId = @ParentDeptId,
                                FactoryId = @FactoryId,
                                Description = @Description,
                                ManagerEmployeeId = @ManagerEmployeeId
                            WHERE DeptId = @DeptId";

             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 // 显式指定 NVarChar
                 command.Parameters.Add("@DeptName", SqlDbType.NVarChar).Value = department.DeptName;
                 command.Parameters.AddWithValue("@ParentDeptId", (object)department.ParentDeptId ?? DBNull.Value);
                 command.Parameters.AddWithValue("@FactoryId", department.FactoryId);
                 // 显式指定 NVarChar
                 command.Parameters.Add("@Description", SqlDbType.NVarChar).Value = (object)department.Description ?? DBNull.Value;
                 command.Parameters.AddWithValue("@ManagerEmployeeId", (object)department.ManagerEmployeeId ?? DBNull.Value);
                 command.Parameters.AddWithValue("@DeptId", department.DeptId);

                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("更新部门信息失败: " + ex.Message, ex);
                 }
             }
        }

        public bool DeleteDepartment(string deptId)
        {
            // 注意：删除部门前可能需要检查是否有员工、子部门等关联
             string query = "DELETE FROM Department WHERE DeptId = @DeptId";
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@DeptId", deptId);
                 try
                 {
                     connection.Open();
                     int result = command.ExecuteNonQuery();
                     return result > 0;
                 }
                 catch (SqlException ex) when (ex.Number == 547) // FK constraint violation
                 {
                     throw new Exception("删除部门失败：该部门下可能存在关联的员工或子部门。", ex);
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("删除部门失败: " + ex.Message, ex);
                 }
             }
        }

        /// <summary>
        /// 检查部门是否有关联的用户
        /// </summary>
        public bool IsDepartmentAssociatedWithUser(string deptId)
        {
            // 注意: 这个查询只检查直接关联，如果需要检查子部门，逻辑应更复杂或在 BLL 处理
            string query = "SELECT COUNT(*) FROM Employee WHERE DeptId = @DeptId"; // 假设用户关联表是 Employee
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@DeptId", deptId);
                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error checking user association for dept {deptId}: {ex.Message}");
                    return true; // 出错时保守处理，认为有关联
                }
            }
        }

        /// <summary>
        /// 检查部门是否有子部门
        /// </summary>
        public bool HasChildDepartments(string parentDeptId)
        {
             string query = "SELECT COUNT(*) FROM Department WHERE ParentDeptId = @ParentDeptId";
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@ParentDeptId", parentDeptId);
                 try
                 {
                     connection.Open();
                     int count = (int)command.ExecuteScalar();
                     return count > 0;
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine($"Error checking child departments for parent {parentDeptId}: {ex.Message}");
                     return true; // 出错时保守处理
                 }
             }
        }

        private Department MapReaderToDepartment(SqlDataReader reader)
        {
            return new Department
            {
                DeptId = reader["DeptId"].ToString(),
                DeptName = reader["DeptName"].ToString(),
                ParentDeptId = reader.IsDBNull(reader.GetOrdinal("ParentDeptId")) ? null : reader["ParentDeptId"].ToString(),
                FactoryId = reader["FactoryId"].ToString(),
                Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString(),
                CreateTime = Convert.ToDateTime(reader["CreateTime"]),
                ManagerEmployeeId = reader.IsDBNull(reader.GetOrdinal("ManagerEmployeeId")) ? null : reader["ManagerEmployeeId"].ToString(),
                ManagerName = reader.IsDBNull(reader.GetOrdinal("ManagerName")) ? null : reader["ManagerName"].ToString()
            };
        }
    }
} 