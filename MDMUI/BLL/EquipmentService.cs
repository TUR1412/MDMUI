using System;
using System.Collections.Generic;
using System.Data;
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    public class EquipmentService
    {
        private EquipmentDAL equipmentDAL;

        public EquipmentService()
        {
            equipmentDAL = new EquipmentDAL();
        }

        /// <summary>
        /// 获取所有设备列表
        /// </summary>
        public DataTable GetAllEquipment()
        {
            return equipmentDAL.GetAllEquipment();
        }

        /// <summary>
        /// 根据ID获取设备信息
        /// </summary>
        public Equipment GetEquipmentById(string equipmentId)
        {
            return equipmentDAL.GetEquipmentById(equipmentId);
        }

        /// <summary>
        /// 添加新设备
        /// </summary>
        public bool AddEquipment(Equipment equipment, User currentUser)
        {
            // 记录操作用户
            equipment.EventUser = currentUser.Username;
            // CreateTime已在模型中设置默认值，无需手动设置EditTime
            
            try
            {
                return equipmentDAL.InsertEquipment(equipment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"添加设备失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 更新设备信息
        /// </summary>
        public bool UpdateEquipment(Equipment equipment, User currentUser)
        {
            // 记录操作用户
            equipment.EventUser = currentUser.Username;
            // CreateTime不更新，且不使用EditTime字段
            
            try
            {
                return equipmentDAL.UpdateEquipment(equipment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"更新设备失败: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 删除设备
        /// </summary>
        public bool DeleteEquipment(string equipmentId, User currentUser)
        {
            try
            {
                return equipmentDAL.DeleteEquipment(equipmentId, currentUser.Username);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"删除设备失败: {ex.Message}");
                return false;
            }
        }
        
        // TODO: Add methods for getting equipment types, eqp groups etc. for filters if needed
        // public List<string> GetAllEquipmentTypes() { ... }
        // public List<EqpGroup> GetAllEqpGroupsForFilter() { ... } // Might use EqpGroupService instance

    }
} 