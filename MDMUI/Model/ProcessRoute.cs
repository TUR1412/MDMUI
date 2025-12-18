using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 工艺路线模型
    /// </summary>
    public class ProcessRoute
    {
        /// <summary>
        /// 工艺路线ID
        /// </summary>
        public string RouteId { get; set; }

        /// <summary>
        /// 工艺站编号
        /// </summary>
        public string StationId { get; set; }

        /// <summary>
        /// 工艺站版本
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// 关联工艺流程ID
        /// </summary>
        public string ProcessId { get; set; }
        
        /// <summary>
        /// 工艺站说明
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// 工艺站顺序
        /// </summary>
        public int Sequence { get; set; }
        
        /// <summary>
        /// 工艺站类型
        /// </summary>
        public string StationType { get; set; }

        /// <summary>
        /// 所在区域
        /// </summary>
        public string Area { get; set; }
        
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        
        /// <summary>
        /// 状态（是否启用）
        /// </summary>
        public string Status { get; set; }
    }
} 