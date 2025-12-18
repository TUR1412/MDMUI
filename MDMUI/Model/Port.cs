using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 端口信息模型类 (对应 port 表)
    /// </summary>
    public class Port
    {
        /// <summary>
        /// 端口编号
        /// </summary>
        public string PortId { get; set; }

        /// <summary>
        /// 端口名称
        /// </summary>
        public string PortName { get; set; }

        /// <summary>
        /// 端口类型
        /// </summary>
        public string PortType { get; set; }

        /// <summary>
        /// 端口物理编号
        /// </summary>
        public string PortNumber { get; set; }

        /// <summary>
        /// 通信协议
        /// </summary>
        public string Protocol { get; set; }

        /// <summary>
        /// 所属子设备ID
        /// </summary>
        public string SubDeviceId { get; set; }

        /// <summary>
        /// 所属子设备ID (别名，与SubDeviceId保持一致)
        /// </summary>
        public string ParentDeviceId { get; set; }

        /// <summary>
        /// 端口描述
        /// </summary>
        public string PortDescription { get; set; }

        /// <summary>
        /// 端口状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 端口地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 端口参数配置
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 创建用户ID
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 编辑用户ID
        /// </summary>
        public string EditUser { get; set; }

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
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 事件类型 (如 Create, Update, Delete)
        /// </summary>
        public string EventType { get; set; }

        // -- 以下为非数据库字段，用于UI显示或关联查询 --
        
        /// <summary>
        /// 所属子设备名称
        /// </summary>
        public string SubDeviceName { get; set; }
    }
} 