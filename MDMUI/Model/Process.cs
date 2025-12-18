using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 工艺流程模型
    /// </summary>
    public class Process
    {
        /// <summary>
        /// 工艺流程ID
        /// </summary>
        public string ProcessId { get; set; }

        /// <summary>
        /// 工艺流程版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 关联工艺包ID
        /// </summary>
        public string PackageId { get; set; }

        /// <summary>
        /// 工艺流程描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 生产类型
        /// </summary>
        public string ProductionType { get; set; }

        /// <summary>
        /// 优先级/顺序
        /// </summary>
        public int Sequence { get; set; }

        /// <summary>
        /// 是否当前使用
        /// </summary>
        public bool IsCurrentlyUsed { get; set; }
    }
} 