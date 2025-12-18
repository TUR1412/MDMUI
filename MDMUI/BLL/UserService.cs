using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility; // For PasswordEncryptor if needed

namespace MDMUI.BLL
{
    /// <summary>
    /// 用户管理业务逻辑服务类
    /// </summary>
    public class UserService
    {
        private UserDAL userDal = new UserDAL();
        private RolesDAL rolesDal = new RolesDAL(); // Instantiate RolesDAL
        private SystemLogBLL systemLogBll = new SystemLogBLL(); // For logging

        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="username">用户名</param>
        /// <param name="password">密码 (明文)</param>
        /// <returns>如果验证成功，返回User对象；否则返回null</returns>
        public User AuthenticateUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                return null;
            }

            try
            {
                // Call ValidateUser instead of GetUserByUsername
                User user = userDal.ValidateUser(username, password);
                if (user != null)
                {
                    // Validation logic is now inside ValidateUser, just need to update LastLoginTime
                    userDal.UpdateLastLoginTime(user.Id);
                    return user;
                }
                return null; // User validation failed
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error authenticating user: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="usernameFilter">用户名过滤 (可选)</param>
        /// <param name="roleFilter">角色过滤 (可选)</param>
        /// <returns>包含用户信息的DataTable</returns>
        public List<User> GetUserList(string usernameFilter = null, string roleFilter = null)
        {
            try
            {
                 // TODO: Implement filtering later, possibly by adding GetUserList to UserDAL
                 // For now, just return all users, ignoring filters.
                return userDal.GetAllUsers(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting user list: " + ex.Message);
                throw; 
            }
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns>角色名称列表</returns>
        public List<Roles> GetAllRoles() // Return List<Roles> instead of List<Role>
        {
            try
            {
                // Call RolesDAL to get roles as DataTable
                DataTable rolesTable = rolesDal.GetAllRoles(); 
                
                // Convert DataTable to List<Roles>
                List<Roles> rolesList = new List<Roles>();
                foreach (DataRow row in rolesTable.Rows)
                {
                    rolesList.Add(new Roles
                    {
                        RoleId = row["RoleId"].ToString(),
                        RoleName = row["RoleName"].ToString(),
                        Description = row["Description"] == DBNull.Value ? string.Empty : row["Description"].ToString(),
                        CreateTime = row.IsNull("CreateTime") ? DateTime.Now : Convert.ToDateTime(row["CreateTime"]),
                        CreateUser = row["CreateUser"] == DBNull.Value ? string.Empty : row["CreateUser"].ToString(),
                        Status = row["Status"] == DBNull.Value ? "Active" : row["Status"].ToString()
                    });
                }
                
                return rolesList;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting roles: " + ex.Message);
                return new List<Roles>(); // Return empty list on error
            }
        }


        /// <summary>
        /// 添加新用户
        /// </summary>
        /// <param name="userToAdd">要添加的用户信息</param>
        /// <param name="currentUser">执行操作的当前用户</param>
        /// <returns>是否添加成功</returns>
        public bool AddUser(User userToAdd, User currentUser)
        {
            if (userToAdd == null) { throw new ArgumentNullException(nameof(userToAdd)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
            // 添加验证逻辑 (例如：用户名是否已存在，密码复杂度等)
            if (string.IsNullOrWhiteSpace(userToAdd.Username)) { throw new ArgumentException("用户名不能为空。", nameof(userToAdd.Username)); }
            if (string.IsNullOrWhiteSpace(userToAdd.Password)) { throw new ArgumentException("密码不能为空。", nameof(userToAdd.Password)); } // 应该检查加密后的密码
             if (string.IsNullOrWhiteSpace(userToAdd.RoleName)) { throw new ArgumentException("角色不能为空。", nameof(userToAdd.RoleName)); }


            try
            {
                // 可以在这里对密码进行加密处理
                // userToAdd.PasswordHash = PasswordEncryptor.HashPassword(userToAdd.Password); // 假设有密码哈希字段

                bool success = userDal.AddUser(userToAdd); // 假设DAL方法接受User对象
                if (success)
                {
                    systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Create", "User", $"用户 [{userToAdd.Username}] 添加成功");
                }
                return success;
            }
            catch (Exception ex) // 处理可能的DAL异常，如用户名重复
            {
                Console.WriteLine("BLL Error adding user: " + ex.Message);
                throw; // 向上抛出给UI层
            }
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userToUpdate">要更新的用户信息</param>
        /// <param name="currentUser">执行操作的当前用户</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateUser(User userToUpdate, User currentUser)
        {
            if (userToUpdate == null) { throw new ArgumentNullException(nameof(userToUpdate)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
            if (userToUpdate.Id <= 0) { throw new ArgumentException("无效的用户ID。", nameof(userToUpdate.Id)); }
             if (string.IsNullOrWhiteSpace(userToUpdate.RoleName)) { throw new ArgumentException("角色不能为空。", nameof(userToUpdate.RoleName)); }
            // 注意：通常更新时不直接更新密码，除非提供了修改密码的特定功能
            // 如果允许在此更新密码，需要添加密码验证和加密逻辑

            try
            {
                bool success = userDal.UpdateUser(userToUpdate); // 假设DAL方法接受User对象
                if (success)
                {
                     systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Update", "User", $"用户 [{userToUpdate.Username}] (ID: {userToUpdate.Id}) 更新成功");
                }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error updating user: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userIdToDelete">要删除的用户ID</param>
        /// <param name="currentUser">执行操作的当前用户</param>
        /// <returns>是否删除成功</returns>
        public bool DeleteUser(int userIdToDelete, User currentUser)
        {
             if (userIdToDelete <= 0) { throw new ArgumentException("无效的用户ID。", nameof(userIdToDelete)); }
             if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
             // 添加业务规则检查，例如不能删除自己，不能删除最后一个管理员等
             if (userIdToDelete == currentUser.Id)
             {
                throw new InvalidOperationException("不能删除当前登录的用户。");
             }
            // 获取用户名用于日志记录 (在删除前获取)
            string usernameToDelete = userDal.GetUserById(userIdToDelete)?.Username ?? $"ID:{userIdToDelete}";


            try
            {
                bool success = userDal.DeleteUser(userIdToDelete);
                if (success)
                {
                     systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Delete", "User", $"用户 [{usernameToDelete}] (ID: {userIdToDelete}) 删除成功");
                }
                 return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error deleting user: " + ex.Message);
                 throw;
            }
        }

         /// <summary>
        /// 根据ID获取单个用户信息
        /// </summary>
        public User GetUserById(int userId)
        {
            if (userId <= 0) { return null;}
            try
            {
                return userDal.GetUserById(userId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error getting user by ID {userId}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// 重置用户密码
        /// </summary>
        /// <param name="userIdToReset">要重置密码的用户ID</param>
        /// <param name="newPassword">新的明文密码</param> // BLL 接收明文，内部加密
        /// <param name="currentUser">执行操作的当前用户</param>
        /// <returns>是否重置成功</returns>
        public bool ResetPassword(int userIdToReset, string newPassword, User currentUser)
        {
            if (userIdToReset <= 0) { throw new ArgumentException("无效的用户ID。", nameof(userIdToReset)); }
            if (string.IsNullOrWhiteSpace(newPassword)) { throw new ArgumentException("新密码不能为空。", nameof(newPassword)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }

            // 获取用户名用于日志
             string usernameToReset = userDal.GetUserById(userIdToReset)?.Username ?? $"ID:{userIdToReset}";

            try
            {
                // 在业务逻辑层加密密码
                string encryptedPassword = PasswordEncryptor.EncryptPassword(newPassword);

                // 调用DAL层执行密码重置
                bool success = userDal.ResetPassword(userIdToReset, encryptedPassword);

                if (success)
                {
                    systemLogBll.AddLog(currentUser.Id, currentUser.Username, "ResetPassword", "User", $"用户 [{usernameToReset}] (ID: {userIdToReset}) 密码重置成功");
                }
                return success;
            }
            catch (Exception ex)
            {
                 Console.WriteLine($"BLL Error resetting password for user ID {userIdToReset}: {ex.Message}");
                 throw; // Re-throw to UI layer
            }
        }
    }
} 