using MDMUI.DAL;
using MDMUI.Model;
using System;
using System.Data;

namespace MDMUI.BLL
{
    public class ProcessPackageBLL
    {
        private readonly ProcessPackageDAL dal;

        public ProcessPackageBLL()
        {
            dal = new ProcessPackageDAL();
        }

        /// <summary>
        /// 获取所有工艺包
        /// </summary>
        /// <returns>工艺包数据表</returns>
        public DataTable GetAllProcessPackages()
        {
            try
            {
                return dal.GetAllProcessPackages();
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层获取所有工艺包失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 根据产品ID获取工艺包
        /// </summary>
        /// <param name="productId">产品ID</param>
        /// <returns>工艺包数据表</returns>
        public DataTable GetProcessPackagesByProductId(string productId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(productId))
                {
                    throw new ArgumentException("产品ID不能为空", nameof(productId));
                }
                
                return dal.GetProcessPackagesByProductId(productId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层根据产品ID获取工艺包失败: {ex.Message}");
                throw;
            }
        }
    }
} 