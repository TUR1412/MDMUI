using System;
using System.Data;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    /// <summary>
    /// 产品业务逻辑层
    /// </summary>
    public class ProductBLL
    {
        private ProductDAL productDAL;

        public ProductBLL()
        {
            productDAL = new ProductDAL();
        }

        /// <summary>
        /// 获取所有产品数据
        /// </summary>
        /// <returns>包含产品数据的DataTable</returns>
        public DataTable GetAllProducts()
        {
            try
            {
                return productDAL.GetAllProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BusinessLayer - 获取产品数据失败: {ex.Message}");
                throw new Exception($"获取产品数据失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 根据ID获取产品
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>产品对象</returns>
        public Product GetProductById(string productId)
        {
            try
            {
                return productDAL.GetProductById(productId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BusinessLayer - 获取产品失败: {ex.Message}");
                throw new Exception($"获取产品失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 搜索产品
        /// </summary>
        /// <param name="productName">产品名称</param>
        /// <param name="productCode">产品编码</param>
        /// <param name="categoryId">类别ID</param>
        /// <returns>符合搜索条件的产品数据</returns>
        public DataTable SearchProducts(string productName = null, string productCode = null, string categoryId = null)
        {
            try
            {
                return productDAL.SearchProducts(productName, productCode, categoryId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BusinessLayer - 搜索产品失败: {ex.Message}");
                throw new Exception($"搜索产品失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 删除产品
        /// </summary>
        public bool DeleteProduct(string productId)
        {
            try
            {
                return productDAL.DeleteProduct(productId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BusinessLayer - 删除产品失败: {ex.Message}");
                throw;
            }
        }
    }
} 
