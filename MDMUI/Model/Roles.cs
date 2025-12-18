using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 角色信息模型类
    /// </summary>
    public class Roles
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建用户ID
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public Roles()
        {
            RoleId = string.Empty;
            RoleName = string.Empty;
            Description = string.Empty;
            CreateTime = DateTime.Now;
            CreateUser = string.Empty;
            Status = "Active";
        }
    }
} 