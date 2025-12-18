using MDMUI.DAL;
using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace MDMUI.BLL
{
    /// <summary>
    /// 用户模块业务逻辑层，处理用户对系统模块的访问权限
    /// </summary>
    public class UserModuleBLL
    {
        private readonly UserDAL userDal;
        private readonly MenuDAL menuDal;

        public UserModuleBLL()
        {
            userDal = new UserDAL();
            menuDal = new MenuDAL();
        }

        /// <summary>
        /// 获取用户可访问的模块列表
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns>用户可访问的模块数据表</returns>
        public DataTable GetAccessibleModules(string userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("用户ID不能为空", nameof(userId));
                }
                
                // 调用数据访问层获取用户可访问的模块
                DataTable modules = menuDal.GetUserAccessibleMenus(userId);
                
                return modules;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"获取用户可访问模块失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 检查用户对指定模块的访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleId">模块ID</param>
        /// <returns>如果用户有访问权限，则返回true；否则返回false</returns>
        public bool CheckModuleAccess(string userId, string moduleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("用户ID不能为空", nameof(userId));
                }
                
                if (string.IsNullOrWhiteSpace(moduleId))
                {
                    throw new ArgumentException("模块ID不能为空", nameof(moduleId));
                }
                
                // 调用数据访问层检查用户是否有权限访问指定模块
                bool hasAccess = menuDal.CheckUserModuleAccess(userId, moduleId);
                
                return hasAccess;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"检查用户模块访问权限失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 为用户分配模块访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleIds">模块ID列表</param>
        /// <returns>如果分配成功，则返回true；否则返回false</returns>
        public bool AssignModulesToUser(string userId, List<string> moduleIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("用户ID不能为空", nameof(userId));
                }
                
                if (moduleIds == null || moduleIds.Count == 0)
                {
                    throw new ArgumentException("模块ID列表不能为空", nameof(moduleIds));
                }
                
                // 调用数据访问层分配模块权限
                bool success = menuDal.AssignModulesToUser(userId, moduleIds);
                
                return success;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"分配用户模块权限失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 移除用户的模块访问权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="moduleIds">模块ID列表</param>
        /// <returns>如果移除成功，则返回true；否则返回false</returns>
        public bool RemoveUserModules(string userId, List<string> moduleIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userId))
                {
                    throw new ArgumentException("用户ID不能为空", nameof(userId));
                }
                
                if (moduleIds == null || moduleIds.Count == 0)
                {
                    throw new ArgumentException("模块ID列表不能为空", nameof(moduleIds));
                }
                
                // 调用数据访问层移除模块权限
                bool success = menuDal.RemoveUserModules(userId, moduleIds);
                
                return success;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"移除用户模块权限失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取所有系统模块列表
        /// </summary>
        /// <returns>所有系统模块的数据表</returns>
        public DataTable GetAllModules()
        {
            try
            {
                // 调用数据访问层获取所有模块
                List<Menu> menuList = menuDal.GetAllMenus();
                
                // 将List<Menu>转换为DataTable
                DataTable modules = new DataTable();
                modules.Columns.Add("MenuId", typeof(string));
                modules.Columns.Add("MenuName", typeof(string));
                modules.Columns.Add("ParentMenuId", typeof(string));
                modules.Columns.Add("MenuOrder", typeof(int));
                modules.Columns.Add("MenuIcon", typeof(string));
                
                foreach (Menu menu in menuList)
                {
                    DataRow row = modules.NewRow();
                    row["MenuId"] = menu.MenuId.ToString();
                    row["MenuName"] = menu.MenuName;
                    row["ParentMenuId"] = menu.ParentMenuId.HasValue ? (object)menu.ParentMenuId.ToString() : DBNull.Value;
                    row["MenuOrder"] = menu.MenuOrder;
                    row["MenuIcon"] = menu.MenuIcon != null ? (object)menu.MenuIcon : DBNull.Value;
                    modules.Rows.Add(row);
                }
                
                return modules;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"获取所有系统模块失败: {ex.Message}");
                throw;
            }
        }
    }
} 