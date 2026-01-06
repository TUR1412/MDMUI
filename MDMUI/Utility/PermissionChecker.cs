using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace MDMUI.Utility
{
    public class PermissionChecker
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        private readonly object cacheGate = new object();
        private int? cachedUserId;
        private HashSet<string> cachedPermissions;

        public void PrimeUserPermissions(int userId)
        {
            if (userId <= 0) return;

            try
            {
                HashSet<string> permissions = LoadUserPermissionsFromDatabase(userId);
                lock (cacheGate)
                {
                    cachedUserId = userId;
                    cachedPermissions = permissions;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PermissionChecker] 预加载权限失败: {ex.Message}");
            }
        }

        public void ClearCache()
        {
            lock (cacheGate)
            {
                cachedUserId = null;
                cachedPermissions = null;
            }
        }

        // 检查用户是否拥有指定权限
        public bool HasPermission(int userId, string moduleName, string actionName)
        {
            try
            {
                if (userId > 0 && !string.IsNullOrWhiteSpace(moduleName) && !string.IsNullOrWhiteSpace(actionName))
                {
                    HashSet<string> snapshot;
                    int? snapshotUserId;
                    lock (cacheGate)
                    {
                        snapshot = cachedPermissions;
                        snapshotUserId = cachedUserId;
                    }

                    if (snapshot != null && snapshotUserId == userId)
                    {
                        return snapshot.Contains(BuildKey(moduleName, actionName));
                    }
                }

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

        private HashSet<string> LoadUserPermissionsFromDatabase(int userId)
        {
            HashSet<string> permissions = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                const string query = @"
SELECT p.ModuleName, p.ActionName
FROM UserPermissions up
JOIN Permissions p ON up.PermissionId = p.PermissionId
WHERE up.UserId = @UserId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@UserId", SqlDbType.Int).Value = userId;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string module = reader.IsDBNull(0) ? null : reader.GetString(0);
                            string action = reader.IsDBNull(1) ? null : reader.GetString(1);
                            if (string.IsNullOrWhiteSpace(module) || string.IsNullOrWhiteSpace(action)) continue;

                            permissions.Add(BuildKey(module, action));
                        }
                    }
                }
            }

            return permissions;
        }

        private static string BuildKey(string moduleName, string actionName)
        {
            return moduleName + "|" + actionName;
        }
    }
} 
