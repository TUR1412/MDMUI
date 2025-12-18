using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL;
using MDMUI.Model;
using System.Diagnostics;

namespace MDMUI.BLL
{
    /// <summary>
    /// 端口业务逻辑服务类
    /// </summary>
    public class PortService
    {
        private readonly PortDAL portDAL;

        public PortService()
        {
            portDAL = new PortDAL();
        }

        /// <summary>
        /// 根据子设备ID获取端口列表
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>包含端口信息的DataTable</returns>
        public DataTable GetPortsByParentDeviceId(string subDeviceId)
        {
            Debug.WriteLine($"PortService: 开始获取子设备[{subDeviceId}]的端口列表");
            try
            {
                if (string.IsNullOrEmpty(subDeviceId))
                {
                    Debug.WriteLine($"PortService: 子设备ID为空，返回空表");
                    DataTable emptyTable = CreateEmptyPortTable();
                    return emptyTable;
                }

                // 获取端口
                DataTable result = portDAL.GetPortsByParentDeviceId(subDeviceId);
                
                // 确保返回的DataTable不为空且包含必要的列
                if (result == null)
                {
                    Debug.WriteLine("PortService: DAL返回了null表，创建空表");
                    result = CreateEmptyPortTable();
                }
                else if (result.Columns.Count == 0)
                {
                    Debug.WriteLine("PortService: DAL返回了无列的表，添加必要的列");
                    AddRequiredColumnsToTable(result);
                }
                
                Debug.WriteLine($"PortService: 成功获取端口数据，共 {result.Rows.Count} 条记录");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PortService异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                
                // 创建一个空表返回，防止UI层出现空引用异常
                DataTable emptyTable = CreateEmptyPortTable();
                return emptyTable;
            }
        }
        
        // 创建一个空的端口表（包含所有必要的列）
        private DataTable CreateEmptyPortTable()
        {
            DataTable dt = new DataTable();
            
            // 添加所需的列
            string[] columns = new string[] {
                "port_id", "port_name", "port_type", "port_number", "protocol",
                "parent_device_id", "create_time", "create_user", "edit_time", 
                "edit_user", "SubDeviceName", "status", "address", "config"
            };
            
            foreach (string column in columns)
            {
                dt.Columns.Add(column);
            }
            
            Debug.WriteLine("PortService: 已创建空的端口数据表");
            return dt;
        }
        
        // 向表中添加必要的列
        private void AddRequiredColumnsToTable(DataTable dt)
        {
            string[] expectedColumns = new string[] {
                "port_id", "port_name", "port_type", "port_number", "protocol",
                "parent_device_id", "create_time", "create_user", "edit_time", 
                "edit_user", "SubDeviceName", "status", "address", "config"
            };
            
            foreach (string column in expectedColumns)
            {
                if (!dt.Columns.Contains(column))
                {
                    dt.Columns.Add(column);
                    Debug.WriteLine($"PortService: 向表添加缺失的列 '{column}'");
                }
            }
        }

        /// <summary>
        /// 获取所有端口列表
        /// </summary>
        /// <returns>包含端口信息的DataTable</returns>
        public DataTable GetAllPorts()
        {
            return portDAL.GetAllPorts();
        }

        /// <summary>
        /// 根据ID获取端口信息
        /// </summary>
        /// <param name="portId">端口ID</param>
        /// <returns>端口对象</returns>
        public DataTable GetPortById(string portId)
        {
            return portDAL.GetPortById(portId);
        }

        /// <summary>
        /// 添加端口
        /// </summary>
        /// <param name="port">端口对象</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool AddPort(Port port, string currentUser)
        {
            try
            {
                port.CreateUser = currentUser;
                port.CreateTime = DateTime.Now;
                return portDAL.AddPort(port);
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"添加端口出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新端口信息
        /// </summary>
        /// <param name="port">端口对象</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdatePort(Port port, string currentUser)
        {
            try
            {
                port.EditUser = currentUser;
                port.EditTime = DateTime.Now;
                return portDAL.UpdatePort(port);
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"更新端口出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除端口
        /// </summary>
        /// <param name="portId">端口ID</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeletePort(string portId, string currentUser)
        {
            try
            {
                return portDAL.DeletePort(portId, currentUser);
            }
            catch (Exception ex)
            {
                // 记录错误
                Console.WriteLine($"删除端口出错: {ex.Message}");
                throw; // 重新抛出异常，以便UI层可以显示详细错误
            }
        }

        /// <summary>
        /// 获取设备下的最大端口编号
        /// </summary>
        public int GetMaxPortNumberForDevice(string subDeviceId)
        {
            try
            {
                int maxPortNumber = portDAL.GetMaxPortNumberForDevice(subDeviceId);
                return maxPortNumber;
            }
            catch
            {
                return 0;
            }
        }
    }
} 