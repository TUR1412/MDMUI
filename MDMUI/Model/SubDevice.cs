using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 子设备信息模型类 (对应 sub_device 表)
    /// </summary>
    public class SubDevice
    {
        /// <summary>
        /// 子设备编号
        /// </summary>
        public string SubDeviceId { get; set; }

        /// <summary>
        /// 子设备名称
        /// </summary>
        public string SubDeviceName { get; set; }

        /// <summary>
        /// 子设备类型
        /// </summary>
        public string SubDeviceType { get; set; }

        /// <summary>
        /// 子设备描述
        /// </summary>
        public string SubDeviceDescription { get; set; }

        /// <summary>
        /// 所属设备组ID
        /// </summary>
        public string EqpGroupId { get; set; }

        /// <summary>
        /// 子设备状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 事件用户
        /// </summary>
        public string EventUser { get; set; }

        /// <summary>
        /// 事件备注
        /// </summary>
        public string EventRemark { get; set; }

        /// <summary>
        /// 编辑发生时间
        /// </summary>
        public DateTime? EditTime { get; set; }

        /// <summary>
        /// 创建发生时间
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 事件类型 (如 Create, Update, Delete)
        /// </summary>
        public string EventType { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 编辑用户
        /// </summary>
        public string EditUser { get; set; }

        // -- 以下为非数据库字段，用于UI显示或关联查询 --
        
        /// <summary>
        /// 所属设备组名称
        /// </summary>
        public string EqpGroupName { get; set; }
    }
} 