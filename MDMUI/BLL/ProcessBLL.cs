using MDMUI.DAL;
using MDMUI.Model;
using System;
using System.Data;

namespace MDMUI.BLL
{
    public class ProcessBLL
    {
        private readonly ProcessDAL dal;

        public ProcessBLL()
        {
            dal = new ProcessDAL();
        }

        /// <summary>
        /// 根据工艺包ID获取工艺流程
        /// </summary>
        /// <param name="packageId">工艺包ID</param>
        /// <returns>工艺流程数据表</returns>
        public DataTable GetProcessesByPackageId(string packageId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(packageId))
                {
                    throw new ArgumentException("工艺包ID不能为空", nameof(packageId));
                }
                
                return dal.GetProcessesByPackageId(packageId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层根据工艺包ID获取工艺流程失败: {ex.Message}");
                throw;
            }
        }
    }
} 