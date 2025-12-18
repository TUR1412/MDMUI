using System;

namespace MDMUI.Model
{
    /// <summary>
    /// 产品类别模型
    /// </summary>
    public class ProductCategory
    {
        /// <summary>
        /// 类别ID
        /// </summary>
        public string CategoryId { get; set; }

        /// <summary>
        /// 类别名称
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 上级类别ID
        /// </summary>
        public string ParentCategoryId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
} 