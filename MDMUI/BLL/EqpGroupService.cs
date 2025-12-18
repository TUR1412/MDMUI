using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL; // 引入 DAL
using MDMUI.Model; // 引入 Model
using MDMUI.BLL; // 引入其他业务逻辑类

namespace MDMUI.BLL
{
    /// <summary>
    /// 设备组业务逻辑服务类
    /// </summary>
    public class EqpGroupService
    {
        private EqpGroupDAL eqpGroupDal = new EqpGroupDAL();

        /// <summary>
        /// 获取设备组列表
        /// </summary>
        /// <param name="eqpGroupType">设备组类型 (可选)</param>
        /// <param name="eqpGroupId">设备组ID (可选)</param>
        /// <returns>包含设备组信息的DataTable</returns>
        public DataTable GetEqpGroupList(string eqpGroupType = null, string eqpGroupId = null)
        {
            try
            {
                return eqpGroupDal.GetEqpGroupList(eqpGroupType, eqpGroupId);
            }
            catch (Exception ex)
            {
                // BLL 层可以进行更具体的异常处理或日志记录
                Console.WriteLine("BLL Error getting EqpGroup list: " + ex.Message);
                // 可以选择包装异常或返回空DataTable等策略
                // return new DataTable(); 
                throw; // 暂时继续向上抛出
            }
        }

        /// <summary>
        /// 获取设备组历史记录
        /// </summary>
        /// <param name="eqpGroupId">设备组ID</param>
        /// <returns>包含设备组历史记录的DataTable</returns>
        public DataTable GetEqpGroupHisList(string eqpGroupId)
        {
            if (string.IsNullOrEmpty(eqpGroupId))
            {
                // BLL 层可以添加参数校验
                // throw new ArgumentNullException(nameof(eqpGroupId), "设备组ID不能为空");
                return new DataTable(); // 或者返回空表
            }

            try
            {
                return eqpGroupDal.GetEqpGroupHisList(eqpGroupId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting EqpGroup history: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 添加设备组
        /// </summary>
        public bool AddEqpGroup(EqpGroup group, User currentUser)
        {
             if (group == null) { throw new ArgumentNullException(nameof(group)); }
             if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
             // 添加基本验证
             if (string.IsNullOrWhiteSpace(group.EqpGroupId))
             {
                 throw new ArgumentException("设备组ID不能为空", nameof(group.EqpGroupId));
             }
             if (string.IsNullOrWhiteSpace(group.EqpGroupType))
             {
                 throw new ArgumentException("设备组类型不能为空", nameof(group.EqpGroupType));
             }
             if (string.IsNullOrWhiteSpace(group.FactoryId))
             {
                 throw new ArgumentException("所属工厂不能为空", nameof(group.FactoryId));
             }

             try
             {
                 // 调用 DAL 方法 (DAL AddEqpGroup currently takes eventUser string, keep it for now or modify DAL too)
                 bool success = eqpGroupDal.AddEqpGroup(group, currentUser.Username);
                 if (success)
                 {
                     // 使用正确的 User ID 和 Name 记录日志
                     new SystemLogBLL().AddLog(currentUser.Id, currentUser.Username, "Create", "EqpGroup", $"设备组 [{group.EqpGroupId}] 添加成功"); 
                 }
                 return success;
             }
             catch(Exception ex) // DAL 层可能会抛出主键冲突等异常
             {
                 Console.WriteLine("BLL Error adding EqpGroup: " + ex.Message);
                 // 可以选择记录日志或重新包装异常
                 // return false; // 或者根据异常类型决定是否返回 false
                 throw; // 将 DAL 层的异常（如主键冲突）继续向上抛给 UI 层处理
             }
        }

        /// <summary>
        /// 更新设备组
        /// </summary>
        public bool UpdateEqpGroup(EqpGroup group, User currentUser)
        {
             if (group == null) { throw new ArgumentNullException(nameof(group)); }
             if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
             // ...

            try
            {
                // DAL UpdateEqpGroup currently takes eventUser string
                bool success = eqpGroupDal.UpdateEqpGroup(group, currentUser.Username);
                if (success)
                {
                    // 使用正确的 User ID 和 Name 记录日志
                     new SystemLogBLL().AddLog(currentUser.Id, currentUser.Username, "Update", "EqpGroup", $"设备组 [{group.EqpGroupId}] 更新成功");
                }
                 return success;
            }
            catch(Exception ex)
            {
                 Console.WriteLine("BLL Error updating EqpGroup: " + ex.Message);
                 // Re-throw the exception instead of returning false
                 throw; 
                 // return false; 
            }
        }

        /// <summary>
        /// 删除设备组
        /// </summary>
        public bool DeleteEqpGroup(string eqpGroupId, User currentUser)
        {
            if (string.IsNullOrEmpty(eqpGroupId)) { throw new ArgumentNullException(nameof(eqpGroupId)); }
            if (currentUser == null) { throw new ArgumentNullException(nameof(currentUser)); }
            // ...

             try
            {
                 // DAL DeleteEqpGroup currently takes eventUser string
                 bool success = eqpGroupDal.DeleteEqpGroup(eqpGroupId, currentUser.Username);
                 if (success)
                 {
                     // 使用正确的 User ID 和 Name 记录日志
                     new SystemLogBLL().AddLog(currentUser.Id, currentUser.Username, "Delete", "EqpGroup", $"设备组 [{eqpGroupId}] 删除成功");
                 }
                  return success;
            }
            catch(Exception ex)
            {
                 Console.WriteLine("BLL Error deleting EqpGroup: " + ex.Message);
                 // Re-throw the exception instead of returning false
                 throw;
                 // return false;
            }
        }
        
        /// <summary>
        /// 根据ID获取单个设备组信息
        /// </summary>
        public EqpGroup GetEqpGroupById(string eqpGroupId)
        {
            if (string.IsNullOrEmpty(eqpGroupId))
            {
                return null;
            }
             try
            {
                 return eqpGroupDal.GetEqpGroupById(eqpGroupId);
            }
            catch(Exception ex)
            {
                 Console.WriteLine("BLL Error getting EqpGroup by ID: " + ex.Message);
                 return null;
            }
        }

        /// <summary>
        /// 获取所有不同的设备组类型
        /// </summary>
        public List<string> GetDistinctEqpGroupTypes()
        {
            try
            {
                return eqpGroupDal.GetDistinctEqpGroupTypes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting distinct EqpGroup types: " + ex.Message);
                // 返回空列表，让UI能处理
                return new List<string>(); 
            }
        }

        /// <summary>
        /// 获取所有设备组的基础信息 (ID 和 Description) 用于筛选下拉列表
        /// </summary>
        public List<EqpGroup> GetAllEqpGroupsForFilter()
        {
            try
            {
                return eqpGroupDal.GetAllEqpGroupsBasicInfo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting EqpGroup basic info for filter: " + ex.Message);
                return new List<EqpGroup>(); // 返回空列表，避免UI层出错
            }
        }
    }
} 