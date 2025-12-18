using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MDMUI.Model; // Assuming Permission model exists in Model folder
using System.Linq;

namespace MDMUI.DAL
{
    /// <summary>
    /// 数据访问类 - 处理权限和用户权限数据
    /// </summary>
    public class PermissionDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 获取所有定义的权限
        /// </summary>
        /// <returns>所有权限对象的列表</returns>
        public List<Permission> GetAllPermissions()
        {
            List<Permission> permissions = new List<Permission>();
            string query = "SELECT PermissionId, ModuleName, ActionName, Description FROM Permissions ORDER BY ModuleName, ActionName";

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
                            permissions.Add(new Permission
                            {
                                PermissionId = (int)reader["PermissionId"],
                                ModuleName = reader["ModuleName"].ToString(),
                                ActionName = reader["ActionName"].ToString(),
                                Description = reader["Description"]?.ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Consider logging the exception
                    Console.WriteLine("Error getting all permissions: " + ex.Message);
                    throw new Exception("获取所有权限数据失败。", ex);
                }
            }
            return permissions;
        }

        /// <summary>
        /// 获取指定用户拥有的所有权限ID
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户拥有的权限ID集合</returns>
        public HashSet<int> GetUserPermissionIds(int userId)
        {
            HashSet<int> permissionIds = new HashSet<int>();
            string query = "SELECT PermissionId FROM UserPermissions WHERE UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            permissionIds.Add((int)reader["PermissionId"]);
                        }
                    }
                }
                catch (Exception ex)
                {
                     // Consider logging the exception
                    Console.WriteLine($"Error getting permissions for user {userId}: {ex.Message}");
                    throw new Exception($"获取用户 {userId} 的权限数据失败。", ex);
                }
            }
            return permissionIds;
        }

        /// <summary>
        /// 保存指定用户的权限设置（覆盖式）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionIds">该用户应拥有的权限ID列表</param>
        /// <returns>是否保存成功</returns>
        public bool SaveUserPermissions(int userId, List<int> permissionIds)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlTransaction transaction = connection.BeginTransaction(); // Start transaction

                try
                {
                    // 1. Delete existing permissions for the user
                    string deleteQuery = "DELETE FROM UserPermissions WHERE UserId = @UserId";
                    using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection, transaction))
                    {
                        deleteCommand.Parameters.AddWithValue("@UserId", userId);
                        deleteCommand.ExecuteNonQuery();
                    }

                    // 2. Insert new permissions if any
                    if (permissionIds != null && permissionIds.Count > 0)
                    {
                        // Prepare insert statement
                        string insertQuery = "INSERT INTO UserPermissions (UserId, PermissionId) VALUES (@UserId, @PermissionId)";
                        using (SqlCommand insertCommand = new SqlCommand(insertQuery, connection, transaction))
                        {
                            // Add parameters once
                            insertCommand.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;
                            insertCommand.Parameters.Add("@PermissionId", SqlDbType.Int); 

                            // Execute for each permission ID
                            foreach (int permissionId in permissionIds)
                            {
                                insertCommand.Parameters["@PermissionId"].Value = permissionId;
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit(); // Commit transaction if all operations succeed
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback transaction on error
                     // Consider logging the exception
                    Console.WriteLine($"Error saving permissions for user {userId}: {ex.Message}");
                    throw new Exception($"保存用户 {userId} 的权限失败。", ex);
                    // return false; // Or rethrow the exception
                }
            }
        }
    }
} 