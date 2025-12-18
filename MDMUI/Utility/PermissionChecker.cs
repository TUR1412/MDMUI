using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDMUI.Utility
{
    public class PermissionChecker
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        // 检查用户是否拥有指定权限
        public bool HasPermission(int userId, string moduleName, string actionName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = @"
                        SELECT COUNT(*)
                        FROM UserPermissions up
                        JOIN Permissions p ON up.PermissionId = p.PermissionId
                        WHERE up.UserId = @UserId
                        AND p.ModuleName = @ModuleName
                        AND p.ActionName = @ActionName";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@ModuleName", moduleName);
                    command.Parameters.AddWithValue("@ActionName", actionName);

                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                // 出现异常时，为了安全，默认没有权限
                Debug.WriteLine($"[PermissionChecker] 权限检查失败: {ex.Message}");
                return false;
            }
        }
    }
} 
