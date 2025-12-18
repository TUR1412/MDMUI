using MDMUI.DAL;
using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace MDMUI.BLL
{
    /// <summary>
    /// 角色业务逻辑层，处理角色相关的业务逻辑
    /// </summary>
    public class RoleBLL
    {
        private readonly RolesDAL roleDal;
        private readonly MenuDAL menuDal;

        public RoleBLL()
        {
            roleDal = new RolesDAL();
            menuDal = new MenuDAL();
        }

        /// <summary>
        /// 获取所有角色列表
        /// </summary>
        /// <returns>所有角色的数据表</returns>
        public DataTable GetAllRoles()
        {
            try
            {
                return roleDal.GetAllRoles();
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"获取所有角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据ID获取角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色对象</returns>
        public Roles GetRoleById(string roleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    throw new ArgumentException("角色ID不能为空", nameof(roleId));
                }
                
                return roleDal.GetRoleById(roleId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"根据ID获取角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>如果添加成功，则返回角色ID；否则返回空字符串</returns>
        public string AddRole(Roles role)
        {
            try
            {
                if (role == null)
                {
                    throw new ArgumentNullException(nameof(role));
                }
                
                if (string.IsNullOrWhiteSpace(role.RoleName))
                {
                    throw new ArgumentException("角色名称不能为空", nameof(role.RoleName));
                }
                
                // 检查角色名是否已经存在
                bool exists = roleDal.CheckRoleNameExists(role.RoleName);
                if (exists)
                {
                    throw new InvalidOperationException($"角色名 '{role.RoleName}' 已经存在");
                }
                
                // 生成角色ID
                if (string.IsNullOrEmpty(role.RoleId))
                {
                    role.RoleId = Guid.NewGuid().ToString("N");
                }
                
                // 设置创建时间
                role.CreateTime = DateTime.Now;
                
                // 添加角色
                bool success = roleDal.AddRole(role);
                
                return success ? role.RoleId : string.Empty;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"添加角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="role">角色对象</param>
        /// <returns>如果更新成功，则返回true；否则返回false</returns>
        public bool UpdateRole(Roles role)
        {
            try
            {
                if (role == null)
                {
                    throw new ArgumentNullException(nameof(role));
                }
                
                if (string.IsNullOrWhiteSpace(role.RoleId))
                {
                    throw new ArgumentException("角色ID不能为空", nameof(role.RoleId));
                }
                
                if (string.IsNullOrWhiteSpace(role.RoleName))
                {
                    throw new ArgumentException("角色名称不能为空", nameof(role.RoleName));
                }
                
                // 检查角色ID是否存在
                Roles existingRole = roleDal.GetRoleById(role.RoleId);
                if (existingRole == null)
                {
                    throw new InvalidOperationException($"角色ID '{role.RoleId}' 不存在");
                }
                
                // 检查更新的角色名是否与其他角色冲突
                bool nameExists = roleDal.CheckRoleNameExistsExcludeSelf(role.RoleName, role.RoleId);
                if (nameExists)
                {
                    throw new InvalidOperationException($"角色名 '{role.RoleName}' 已经被其他角色使用");
                }
                
                // 更新角色
                return roleDal.UpdateRole(role);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"更新角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>如果删除成功，则返回true；否则返回false</returns>
        public bool DeleteRole(string roleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    throw new ArgumentException("角色ID不能为空", nameof(roleId));
                }
                
                // 检查角色是否被用户使用
                bool isUsed = roleDal.CheckRoleIsUsed(roleId);
                if (isUsed)
                {
                    throw new InvalidOperationException("该角色已被用户使用，无法删除");
                }
                
                // 删除角色
                return roleDal.DeleteRole(roleId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"删除角色失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 获取角色的权限列表
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <returns>角色权限的数据表</returns>
        public DataTable GetRolePermissions(string roleId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    throw new ArgumentException("角色ID不能为空", nameof(roleId));
                }
                
                return roleDal.GetRolePermissions(roleId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"获取角色权限失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 分配权限给角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="permissionIds">权限ID列表</param>
        /// <returns>如果分配成功，则返回true；否则返回false</returns>
        public bool AssignPermissionsToRole(string roleId, List<string> permissionIds)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(roleId))
                {
                    throw new ArgumentException("角色ID不能为空", nameof(roleId));
                }
                
                if (permissionIds == null)
                {
                    throw new ArgumentNullException(nameof(permissionIds));
                }
                
                return roleDal.AssignPermissionsToRole(roleId, permissionIds);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"分配角色权限失败: {ex.Message}");
                throw;
            }
        }
    }
} 