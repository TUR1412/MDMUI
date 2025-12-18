using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 设备组信息模型类 (对应 eqp_group 表)
    /// </summary>
    public class EqpGroup
    {
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
        /// 工厂名称 (需要连接查询获取)
        /// </summary>
        public string FactoryName { get; set; }

        /// <summary>
        /// 用于 ComboBox 显示的组合信息 (格式: ID - Description)
        /// </summary>
        public string DisplayInfo
        {
            get
            {
                // 如果 description 为空或空白，只显示 ID
                if (string.IsNullOrWhiteSpace(EqpGroupDescription))
                {
                    return EqpGroupId;
                }
                return $"{EqpGroupId} - {EqpGroupDescription}";
            }
        }
    }
} 