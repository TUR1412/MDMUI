using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 区域模型
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        public string AreaId { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName { get; set; }

        /// <summary>
        /// 上级区域ID
        /// </summary>
        public string ParentAreaId { get; set; }

        /// <summary>
        /// 邮政编码
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
} 