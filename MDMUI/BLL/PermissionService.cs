using System;
using System.Collections.Generic;
using System.Linq;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    /// <summary>
    /// 业务逻辑类 - 处理权限设置
    /// </summary>
    public class PermissionService
    {
        private PermissionDAL permissionDal = new PermissionDAL();
        private SystemLogBLL systemLogBll = new SystemLogBLL();
        // Need UserDAL to get username for logging
        private UserDAL userDal = new UserDAL(); 

        /// <summary>
        /// 获取所有定义的权限
        /// </summary>
        public List<Permission> GetAllPermissions()
        {
            try
            {
                return permissionDal.GetAllPermissions();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting all permissions: " + ex.Message);
                // Depending on UI handling, maybe return empty list
                // return new List<Permission>(); 
                throw; // Rethrow for now
            }
        }

        /// <summary>
        /// 获取指定用户拥有的权限ID集合
        /// </summary>
        public HashSet<int> GetUserPermissionIds(int userId)
        {
            try
            {
                return permissionDal.GetUserPermissionIds(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error getting permissions for user {userId}: {ex.Message}");
                // Depending on UI handling, maybe return empty set
                // return new HashSet<int>();
                throw; // Rethrow for now
            }
        }

        /// <summary>
        /// 保存指定用户的权限设置，并记录日志
        /// </summary>
        /// <param name="userId">目标用户ID</param>
        /// <param name="permissionIds">要赋予的权限ID列表</param>
        /// <param name="currentUser">执行操作的用户</param>
        /// <returns>是否保存成功</returns>
        public bool SaveUserPermissions(int userId, List<int> permissionIds, User currentUser)
        {
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
            if (userId <= 0) { throw new ArgumentException("Invalid target user ID.", nameof(userId)); }

            // Get target username for logging (best effort)
            string targetUsername = "";
            try 
            {
                targetUsername = userDal.GetUserById(userId)?.Username ?? $"ID:{userId}";
            }
            catch (Exception ex) 
            {
                 Console.WriteLine($"BLL Warning: Could not get target username for logging user permission save (UserId: {userId}): {ex.Message}");
                 targetUsername = $"ID:{userId}"; // Fallback to ID
            }

            try
            {
                bool success = permissionDal.SaveUserPermissions(userId, permissionIds);
                if (success)
                {
                    // --- Generate detailed log description --- 
                    string description = $"用户 [{targetUsername}] 的权限已更新。";
                    if (permissionIds != null && permissionIds.Count > 0)
                    {
                        try
                        {
                            // Get details for the granted permissions
                            var allPermissions = permissionDal.GetAllPermissions(); // Get all permission definitions
                            var grantedPermissions = allPermissions
                                .Where(p => permissionIds.Contains(p.PermissionId))
                                .ToList();

                            if (grantedPermissions.Any())
                            {
                                // Group by module and format the string
                                var grouped = grantedPermissions.GroupBy(p => p.ModuleName)
                                                              .OrderBy(g => g.Key); // Optional: Order modules alphabetically
                                
                                var permissionDetails = new System.Text.StringBuilder("授予权限: ");
                                foreach (var group in grouped)
                                {
                                    permissionDetails.Append($"{group.Key}(");
                                    permissionDetails.Append(string.Join(", ", group.Select(p => p.ActionName).OrderBy(a => a))); // Optional: Order actions alphabetically
                                    permissionDetails.Append("), ");
                                }
                                // Remove trailing comma and space
                                if (permissionDetails.Length > "授予权限: ".Length)
                                {
                                    permissionDetails.Length -= 2;
                                }
                                description += " " + permissionDetails.ToString();
                            }
                            else 
                            {
                                 description += $" 但未能获取具体权限信息。授予权限数量: {permissionIds.Count}"; // Fallback if filtering fails
                            }
                        }
                        catch (Exception logEx)
                        {
                            Console.WriteLine($"BLL Warning: Failed to generate detailed permission log description: {logEx.Message}");
                            description += $" 授予权限数量: {permissionIds.Count}"; // Fallback description
                        }
                    }
                    else
                    {
                        description += " 未授予任何权限。";
                    }
                    // --- End of detailed log description generation ---
                    
                    systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Update", "Permission", description);
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error saving permissions for user {userId}: {ex.Message}");
                // Rethrow the exception to let the UI know about the failure
                throw; 
                // return false; // Or return false depending on desired error handling
            }
        }
    }
} 
