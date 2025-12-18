using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL; // Reference DAL

namespace MDMUI.BLL
{
    /// <summary>
    /// 系统日志业务逻辑类
    /// </summary>
    public class SystemLogBLL
    {
        private SystemLogDAL systemLogDal = new SystemLogDAL();

        /// <summary>
        /// 获取系统日志记录 (支持过滤)
        /// </summary>
        public DataTable GetSystemLogs(DateTime? startDate, DateTime? endDate, int? userId, string operationType, string operationModule)
        {
            try
            {
                // Basic validation or logic can be added here if needed
                return systemLogDal.GetSystemLogs(startDate, endDate, userId, operationType, operationModule);
            }
            catch (Exception ex)
            {
                // Log or handle BLL specific errors
                Console.WriteLine("BLL Error getting System Logs: " + ex.Message);
                throw; // Rethrow for UI layer to handle
            }
        }

        /// <summary>
        /// 获取日志中所有不同的用户名和ID
        /// </summary>
        public Dictionary<int, string> GetDistinctLogUsers()
        {
            try
            {
                return systemLogDal.GetDistinctLogUsers();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting distinct log users: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 获取日志中所有不同的操作模块
        /// </summary>
        public List<string> GetDistinctLogModules()
        {
            try
            {
                return systemLogDal.GetDistinctLogModules();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting distinct log modules: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 添加一条系统日志记录
        /// </summary>
        public void AddLog(int userId, string userName, string operationType, string operationModule, string description, string ipAddress = null)
        {
            try
            {
                // Add validation or pre-processing if needed
                systemLogDal.AddLog(userId, userName, operationType, operationModule, description, ipAddress);
            }
            catch (Exception ex)
            {
                // Log or handle BLL specific errors related to adding logs
                Console.WriteLine("BLL Error adding System Log: " + ex.Message);
                // Decide if this error should stop the user operation or just be logged
            }
        }
    }
} 