using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL;
using MDMUI.Model;
using System.Diagnostics;

namespace MDMUI.BLL
{
    /// <summary>
    /// 子设备业务逻辑服务类
    /// </summary>
    public class SubDeviceService
    {
        private readonly SubDeviceDAL subDeviceDAL;

        public SubDeviceService()
        {
            subDeviceDAL = new SubDeviceDAL();
        }

        /// <summary>
        /// 根据设备组ID获取子设备列表
        /// </summary>
        /// <param name="eqpGroupId">设备组ID</param>
        /// <returns>包含子设备信息的DataTable</returns>
        public DataTable GetSubDevicesByGroupId(string eqpGroupId)
        {
            Debug.WriteLine($"SubDeviceService: 开始获取设备组[{eqpGroupId}]的子设备列表");
            try
            {
                if (string.IsNullOrEmpty(eqpGroupId))
                {
                    Debug.WriteLine($"SubDeviceService: 设备组ID为空，返回空表");
                    DataTable emptyTable = CreateEmptySubDeviceTable();
                    return emptyTable;
                }

                // 获取子设备
                DataTable result = subDeviceDAL.GetSubDevicesByGroupId(eqpGroupId);
                
                // 确保返回的DataTable不为空且包含必要的列
                if (result == null)
                {
                    Debug.WriteLine("SubDeviceService: DAL返回了null表，创建空表");
                    result = CreateEmptySubDeviceTable();
                }
                else if (result.Columns.Count == 0)
                {
                    Debug.WriteLine("SubDeviceService: DAL返回了无列的表，添加必要的列");
                    AddRequiredColumnsToTable(result);
                }
                
                Debug.WriteLine($"SubDeviceService: 成功获取子设备数据，共 {result.Rows.Count} 条记录");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SubDeviceService异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine(ex.StackTrace);
                
                // 创建一个空表返回，防止UI层出现空引用异常
                DataTable emptyTable = CreateEmptySubDeviceTable();
                return emptyTable;
            }
        }
        
        // 创建一个空的子设备表（包含所有必要的列）
        private DataTable CreateEmptySubDeviceTable()
        {
            DataTable dt = new DataTable();
            
            // 添加所需的列
            string[] columns = new string[] {
                "sub_device_id", "sub_device_name", "sub_device_type", "sub_device_description",
                "eqp_group_id", "create_time", "create_user", "edit_time", "edit_user", "EqpGroupName"
            };
            
            foreach (string column in columns)
            {
                dt.Columns.Add(column);
            }
            
            Debug.WriteLine("SubDeviceService: 已创建空的子设备数据表");
            return dt;
        }
        
        // 向表中添加必要的列
        private void AddRequiredColumnsToTable(DataTable dt)
        {
            string[] expectedColumns = new string[] {
                "sub_device_id", "sub_device_name", "sub_device_type", "sub_device_description",
                "eqp_group_id", "create_time", "create_user", "edit_time", "edit_user", "EqpGroupName"
            };
            
            foreach (string column in expectedColumns)
            {
                if (!dt.Columns.Contains(column))
                {
                    dt.Columns.Add(column);
                    Debug.WriteLine($"SubDeviceService: 向表添加缺失的列 '{column}'");
                }
            }
        }

        /// <summary>
        /// 获取所有子设备列表
        /// </summary>
        /// <returns>包含子设备信息的DataTable</returns>
        public DataTable GetAllSubDevices()
        {
            return subDeviceDAL.GetAllSubDevices();
        }

        /// <summary>
        /// 根据ID获取子设备信息
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>子设备对象</returns>
        public DataTable GetSubDeviceById(string subDeviceId)
        {
            return subDeviceDAL.GetSubDeviceById(subDeviceId);
        }

        /// <summary>
        /// 添加子设备
        /// </summary>
        /// <param name="subDevice">子设备对象</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool AddSubDevice(SubDevice subDevice, string currentUser)
        {
            if (subDevice == null)
            {
                throw new ArgumentNullException(nameof(subDevice));
            }

            if (string.IsNullOrEmpty(currentUser))
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            // 验证必填字段
            if (string.IsNullOrEmpty(subDevice.SubDeviceId))
            {
                throw new ArgumentException("子设备ID不能为空", nameof(subDevice.SubDeviceId));
            }

            if (string.IsNullOrEmpty(subDevice.SubDeviceName))
            {
                throw new ArgumentException("子设备名称不能为空", nameof(subDevice.SubDeviceName));
            }

            if (string.IsNullOrEmpty(subDevice.EqpGroupId))
            {
                throw new ArgumentException("所属设备组ID不能为空", nameof(subDevice.EqpGroupId));
            }

            // 设置操作用户信息
            subDevice.CreateUser = currentUser;
            subDevice.CreateTime = DateTime.Now;

            try
            {
                return subDeviceDAL.AddSubDevice(subDevice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加子设备出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新子设备信息
        /// </summary>
        /// <param name="subDevice">子设备对象</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdateSubDevice(SubDevice subDevice, string currentUser)
        {
            if (subDevice == null)
            {
                throw new ArgumentNullException(nameof(subDevice));
            }

            if (string.IsNullOrEmpty(currentUser))
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            // 验证必填字段
            if (string.IsNullOrEmpty(subDevice.SubDeviceId))
            {
                throw new ArgumentException("子设备ID不能为空", nameof(subDevice.SubDeviceId));
            }

            if (string.IsNullOrEmpty(subDevice.SubDeviceName))
            {
                throw new ArgumentException("子设备名称不能为空", nameof(subDevice.SubDeviceName));
            }

            if (string.IsNullOrEmpty(subDevice.EqpGroupId))
            {
                throw new ArgumentException("所属设备组ID不能为空", nameof(subDevice.EqpGroupId));
            }

            // 设置操作用户信息
            subDevice.EditUser = currentUser;
            subDevice.EditTime = DateTime.Now;

            try
            {
                return subDeviceDAL.UpdateSubDevice(subDevice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新子设备出错: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除子设备
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <param name="currentUser">当前用户</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeleteSubDevice(string subDeviceId, string currentUser)
        {
            if (string.IsNullOrEmpty(subDeviceId))
            {
                throw new ArgumentException("子设备ID不能为空", nameof(subDeviceId));
            }

            if (string.IsNullOrEmpty(currentUser))
            {
                throw new ArgumentNullException(nameof(currentUser));
            }

            try
            {
                // 检查子设备是否有关联端口
                if (HasAssociatedPorts(subDeviceId))
                {
                    throw new Exception("无法删除子设备，该设备下存在关联的端口。请先删除所有关联端口。");
                }
                
                return subDeviceDAL.DeleteSubDevice(subDeviceId, currentUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除子设备出错: {ex.Message}");
                throw; // 重新抛出异常，以便UI层可以显示详细错误
            }
        }

        /// <summary>
        /// 检查子设备是否有关联的端口
        /// </summary>
        private bool HasAssociatedPorts(string subDeviceId)
        {
            try
            {
                DataTable ports = subDeviceDAL.GetAssociatedPorts(subDeviceId);
                return ports != null && ports.Rows.Count > 0;
            }
            catch
            {
                // 如果查询出错，保守起见，假设存在关联
                return true;
            }
        }

        /// <summary>
        /// 获取最大子设备ID编号
        /// </summary>
        public int GetMaxSubDeviceId()
        {
            try
            {
                string maxId = subDeviceDAL.GetMaxSubDeviceId();
                
                // 提取数字部分
                if (!string.IsNullOrEmpty(maxId) && maxId.Length >= 3 && maxId.StartsWith("SD"))
                {
                    string numPart = maxId.Substring(2);
                    if (int.TryParse(numPart, out int result))
                    {
                        return result;
                    }
                }
                
                return 0; // 如果没有记录或解析失败，返回0
            }
            catch
            {
                return 0;
            }
        }
    }
} 