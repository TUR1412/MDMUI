using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 工艺包模型
    /// </summary>
    public class ProcessPackage
    {
        /// <summary>
        /// 工艺包ID
        /// </summary>
        public string PackageId { get; set; }

        /// <summary>
        /// 工艺包版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 工艺包描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 关联产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }
} 