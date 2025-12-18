using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 工厂模型
    /// </summary>
    public class Factory
    {
        /// <summary>
        /// 工厂ID
        /// </summary>
        public string FactoryId { get; set; }

        /// <summary>
        /// 工厂名称
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 负责人员工ID (外键关联 Employee)
        /// </summary>
        public string ManagerEmployeeId { get; set; }

        /// <summary>
        /// 负责人姓名 (通过 JOIN 获取)
        /// </summary>
        public string ManagerName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }

        // 注意： dbo.sql 中还有 FactoryExtendInfo 表，其字段可以根据需要稍后添加到此模型或创建单独的模型
    }
} 