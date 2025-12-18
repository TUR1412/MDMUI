using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 产品模型
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 类别ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 价格 (可空)
        /// </summary>
        public decimal? Price { get; set; } // Use nullable decimal for optional price

        /// <summary>
        /// 成本 (可空)
        /// </summary>
        public decimal? Cost { get; set; } // Use nullable decimal for optional cost

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        // 可以添加一个 CategoryName 属性，用于在 DataGridView 中显示，但这通常在 BLL 或查询时填充
        // public string CategoryName { get; set; }
    }
} 