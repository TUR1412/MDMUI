using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 部门模型
    /// </summary>
    public class Department
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        public string DeptId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public string ParentDeptId { get; set; }

        /// <summary>
        /// 上级部门名称（用于显示）
        /// </summary>
        public string ParentDeptName { get; set; }

        /// <summary>
        /// 所属工厂ID
        /// </summary>
        public string FactoryId { get; set; }

        /// <summary>
        /// 所属工厂名称（用于显示）
        /// </summary>
        public string FactoryName { get; set; }

        // /// <summary>
        // /// 负责人 (旧字段)
        // /// </summary>
        // public string Manager { get; set; } // 旧字段，移除或注释掉

        /// <summary>
        /// 负责人员工ID (外键关联 Employee)
        /// </summary>
        public string ManagerEmployeeId { get; set; } // 新字段

        /// <summary>
        /// 负责人姓名 (通过 JOIN 获取)
        /// </summary>
        public string ManagerName { get; set; } // 新增字段，用于显示

        /// <summary>
        /// 负责人（兼容旧字段绑定）
        /// </summary>
        public string Manager
        {
            get => ManagerName;
            set => ManagerName = value;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
} 
