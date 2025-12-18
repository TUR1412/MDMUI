using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 设备信息模型类
    /// </summary>
    public class Equipment
    {
        /// <summary>
        /// 设备编号
        /// </summary>
        public string EquipmentId { get; set; }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName { get; set; }

        /// <summary>
        /// 设备类别ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 所属工厂ID
        /// </summary>
        public string FactoryId { get; set; }

        /// <summary>
        /// 设备型号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 制造商
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime? PurchaseDate { get; set; }

        /// <summary>
        /// 购买价格
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// 设备状态（正常、维修中、停用等）
        /// </summary>
        public string Status { get; set; } = "正常";

        /// <summary>
        /// 所在位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 责任人ID（关联Employee表）
        /// </summary>
        public string ResponsiblePerson { get; set; }

        /// <summary>
        /// 维护周期（天）
        /// </summary>
        public int? MaintenanceCycle { get; set; }

        /// <summary>
        /// 上次维护日期
        /// </summary>
        public DateTime? LastMaintenanceDate { get; set; }

        /// <summary>
        /// 下次维护日期
        /// </summary>
        public DateTime? NextMaintenanceDate { get; set; }

        /// <summary>
        /// 设备描述 - 对应数据库中的Description字段
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        // 以下是非数据库字段，用于UI显示或临时存储

        /// <summary>
        /// 设备类型 (UI字段 - 用于显示或过滤，非数据库字段)
        /// 可能映射到CategoryName或基于Model的派生值
        /// </summary>
        public string EquipmentType { get; set; }

        /// <summary>
        /// 设备详细类型 (UI字段 - 用于显示或过滤，非数据库字段)
        /// </summary>
        public string EquipmentSubType { get; set; }

        /// <summary>
        /// 设备组ID (外键关联 - 非数据库字段)
        /// </summary>
        public string EqpGroupId { get; set; }

        /// <summary>
        /// 设备层次 (UI字段 - 非数据库字段)
        /// </summary>
        public string EquipmentLayer { get; set; }

        /// <summary>
        /// 最后操作用户 (UI字段 - 记录操作人，非数据库字段)
        /// </summary>
        public string EventUser { get; set; }

        /// <summary>
        /// 最后操作备注 (UI字段 - 非数据库字段)
        /// </summary>
        public string EventRemark { get; set; }

        /// <summary>
        /// 设备组名称 (非数据库字段，用于UI显示，通过JOIN或查询填充)
        /// </summary>
        public string EqpGroupName { get; set; }

        /// <summary>
        /// 工厂名称 (非数据库字段，用于UI显示，通过JOIN或查询填充)
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 设备类别名称 (非数据库字段，用于UI显示，通过JOIN或查询填充)
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 责任人姓名 (非数据库字段，用于UI显示，通过JOIN或查询填充)
        /// </summary>
        public string ResponsiblePersonName { get; set; }
    }
} 