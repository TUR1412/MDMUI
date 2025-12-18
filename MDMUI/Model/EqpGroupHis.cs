using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 设备组历史记录模型类 (对应 eqp_group_his 表)
    /// </summary>
    public class EqpGroupHis
    {
        /// <summary>
        /// 历史记录编号 (自增主键)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 设备组编号
        /// </summary>
        public string EqpGroupId { get; set; }

        /// <summary>
        /// 设备组类型
        /// </summary>
        public string EqpGroupType { get; set; }

        /// <summary>
        /// 设备组说明
        /// </summary>
        public string EqpGroupDescription { get; set; }

        /// <summary>
        /// 关联的工厂编号
        /// </summary>
        public string FactoryId { get; set; }

        /// <summary>
        /// 事件用户
        /// </summary>
        public string EventUser { get; set; }

        /// <summary>
        /// 事件备注
        /// </summary>
        public string EventRemark { get; set; }

        /// <summary>
        /// 编辑发生时间 (记录历史时的时间)
        /// </summary>
        public DateTime? EditTime { get; set; }

        /// <summary>
        /// 记录创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 事件类型 (如 Create, Update, Delete)
        /// </summary>
        public string EventType { get; set; }
        
        // -- 以下为非数据库字段，用于UI显示或关联查询 --

        /// <summary>
        /// 工厂名称 (需要连接查询获取)
        /// </summary>
        public string FactoryName { get; set; }
    }
} 