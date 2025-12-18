using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;
using System;
using System.Configuration;

namespace MDMUI.DAL
{
    /// <summary>
    /// 角色数据访问类
    /// </summary>
    public class RolesDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 获取所有角色信息
        /// </summary>
        /// <returns>角色数据表</returns>
        public DataTable GetAllRoles()
        {
            DataTable dt = new DataTable();
            string query = "SELECT RoleId, RoleName, Description, CreateTime, CreateUser, Status FROM Roles";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取角色列表失败: " + ex.Message);
                    throw new Exception("获取角色列表失败: " + ex.Message, ex);
                }
            }
            return dt;
        }

        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色对象</returns>
        public Roles GetRoleById(string roleId)
        {
            Roles role = null;
            string query = "SELECT RoleId, RoleName, Description, CreateTime, CreateUser, Status FROM Roles WHERE RoleId = @RoleId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);
                
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            role = new Roles
                            {
                                RoleId = reader["RoleId"].ToString(),
                                RoleName = reader["RoleName"].ToString(),
                                Description = reader["Description"] == DBNull.Value ? string.Empty : reader["Description"].ToString(),
                                CreateTime = Convert.ToDateTime(reader["CreateTime"]),
                                CreateUser = reader["CreateUser"] == DBNull.Value ? string.Empty : reader["CreateUser"].ToString(),
                                Status = reader["Status"] == DBNull.Value ? "Active" : reader["Status"].ToString()
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取角色信息失败: " + ex.Message);
                    throw new Exception("获取角色信息失败: " + ex.Message, ex);
                }
            }
            return role;
        }

        /// <summary>
        /// 检查角色名称是否存在
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <returns>如果存在则返回true，否则返回false</returns>
        public bool CheckRoleNameExists(string roleName)
        {
            bool exists = false;
            string query = "SELECT COUNT(1) FROM Roles WHERE RoleName = @RoleName";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleName", roleName);
                
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    exists = count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("检查角色名称是否存在失败: " + ex.Message);
                    throw new Exception("检查角色名称是否存在失败: " + ex.Message, ex);
                }
            }
            return exists;
        }

        /// <summary>
        /// 检查角色名称是否存在（排除自身）
        /// </summary>
        /// <param name="roleName">角色名称</param>
        /// <param name="roleId">要排除的角色ID</param>
        /// <returns>如果存在则返回true，否则返回false</returns>
        public bool CheckRoleNameExistsExcludeSelf(string roleName, string roleId)
        {
            bool exists = false;
            string query = "SELECT COUNT(1) FROM Roles WHERE RoleName = @RoleName AND RoleId != @RoleId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleName", roleName);
                command.Parameters.AddWithValue("@RoleId", roleId);
                
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    exists = count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("检查角色名称是否存在失败: " + ex.Message);
                    throw new Exception("检查角色名称是否存在失败: " + ex.Message, ex);
                }
            }
            return exists;
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>如果添加成功则返回true，否则返回false</returns>
        public bool AddRole(Roles role)
        {
            string query = @"INSERT INTO Roles (RoleId, RoleName, Description, CreateTime, CreateUser, Status) 
                            VALUES (@RoleId, @RoleName, @Description, @CreateTime, @CreateUser, @Status)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", role.RoleId);
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@Description", (object)role.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@CreateTime", role.CreateTime);
                command.Parameters.AddWithValue("@CreateUser", (object)role.CreateUser ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", (object)role.Status ?? DBNull.Value);
                
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("添加角色失败: " + ex.Message);
                    throw new Exception("添加角色失败: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>如果更新成功则返回true，否则返回false</returns>
        public bool UpdateRole(Roles role)
        {
            string query = @"UPDATE Roles 
                            SET RoleName = @RoleName, 
                                Description = @Description, 
                                Status = @Status
                            WHERE RoleId = @RoleId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", role.RoleId);
                command.Parameters.AddWithValue("@RoleName", role.RoleName);
                command.Parameters.AddWithValue("@Description", (object)role.Description ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", (object)role.Status ?? DBNull.Value);
                
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("更新角色失败: " + ex.Message);
                    throw new Exception("更新角色失败: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>如果删除成功则返回true，否则返回false</returns>
        public bool DeleteRole(string roleId)
        {
            string query = "DELETE FROM Roles WHERE RoleId = @RoleId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);
                
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("删除角色失败: " + ex.Message);
                    throw new Exception("删除角色失败: " + ex.Message, ex);
                }
            }
        }

        /// <summary>
        /// 检查角色是否被用户使用
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>如果已被使用则返回true，否则返回false</returns>
        public bool CheckRoleIsUsed(string roleId)
        {
            bool isUsed = false;
            string query = "SELECT COUNT(1) FROM UserRoles WHERE RoleId = @RoleId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);
                
                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    isUsed = count > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("检查角色是否被使用失败: " + ex.Message);
                    throw new Exception("检查角色是否被使用失败: " + ex.Message, ex);
                }
            }
            return isUsed;
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>权限数据表</returns>
        public DataTable GetRolePermissions(string roleId)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT p.PermissionId, p.PermissionName, p.ModuleId, p.ActionId, p.Description,
                                CASE WHEN rp.RoleId IS NOT NULL THEN 1 ELSE 0 END AS HasPermission
                            FROM Permission p
                            LEFT JOIN RolePermission rp ON p.PermissionId = rp.PermissionId AND rp.RoleId = @RoleId
                            ORDER BY p.ModuleId, p.ActionId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RoleId", roleId);
                
                try
                {
                    connection.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("获取角色权限失败: " + ex.Message);
                    throw new Exception("获取角色权限失败: " + ex.Message, ex);
                }
            }
            return dt;
        }

        /// <summary>
        /// 分配权限给角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        /// <returns>如果分配成功则返回true，否则返回false</returns>
        public bool AssignPermissionsToRole(string roleId, List<string> permissionIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 删除该角色的所有现有权限
                        string deleteQuery = "DELETE FROM RolePermission WHERE RoleId = @RoleId";
                        using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                        {
                            deleteCommand.Parameters.AddWithValue("@RoleId", roleId);
                            deleteCommand.ExecuteNonQuery();
                        }

                        // 添加新的权限
                        if (permissionIds != null && permissionIds.Count > 0)
                        {
                            string insertQuery = "INSERT INTO RolePermission (RoleId, PermissionId) VALUES (@RoleId, @PermissionId)";
                            foreach (string permissionId in permissionIds)
                            {
                                using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
                                {
                                    insertCommand.Parameters.AddWithValue("@RoleId", roleId);
                                    insertCommand.Parameters.AddWithValue("@PermissionId", permissionId);
                                    insertCommand.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("分配角色权限失败: " + ex.Message);
                        transaction.Rollback();
                        throw new Exception("分配角色权限失败: " + ex.Message, ex);
                    }
                }
            }
        }
    }
} 