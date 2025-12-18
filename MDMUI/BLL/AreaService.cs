using System;
using System.Collections.Generic;
using System.Linq; // 添加对 LINQ 的引用
using MDMUI.DAL;
using MDMUI.Model;

namespace MDMUI.BLL
{
    /// <summary>
    /// 业务逻辑服务类 - 区域管理
    /// </summary>
    public class AreaService
    {
        private AreaDAL areaDal = new AreaDAL();
        // 如果需要记录日志，可以添加 SystemLogBLL 依赖
        // private SystemLogBLL systemLogBll = new SystemLogBLL();
        // private User currentUser; // 如果操作需要记录当前用户

        /// <summary>
        /// 获取所有区域信息
        /// </summary>
        public List<Area> GetAllAreas()
        {
            try
            {
                return areaDal.GetAllAreas();
            }
            catch (Exception ex)
            {
                // BLL层日志记录或异常包装
                Console.WriteLine("BLL Error getting all areas: " + ex.Message);
                throw; // 向上抛出
            }
        }

        /// <summary>
        /// 根据ID获取单个区域信息
        /// </summary>
        public Area GetAreaById(string areaId)
        {
            if (string.IsNullOrEmpty(areaId))
            {
                return null;
            }
            try
            {
                return areaDal.GetAreaById(areaId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"BLL Error getting area by ID {areaId}: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 添加新区域
        /// </summary>
        public bool AddArea(Area area, User currentUser = null) // 可选传入 currentUser 用于日志
        {
            if (area == null) { throw new ArgumentNullException(nameof(area)); }
            // 添加验证逻辑
            if (string.IsNullOrWhiteSpace(area.AreaId)) { throw new ArgumentException("区域ID不能为空", nameof(area.AreaId)); }
            if (string.IsNullOrWhiteSpace(area.AreaName)) { throw new ArgumentException("区域名称不能为空", nameof(area.AreaName)); }
            // 可以添加其他验证，如ID格式、父区域ID是否存在等

            try
            {
                bool success = areaDal.AddArea(area);
                // if (success && currentUser != null && systemLogBll != null)
                // {
                //     systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Create", "Area", $"区域 [{area.AreaName}] ({area.AreaId}) 添加成功");
                // }
                return success;
            }
            catch (Exception ex) // 捕获DAL层可能抛出的主键冲突等异常
            {
                Console.WriteLine("BLL Error adding area: " + ex.Message);
                throw; // 将异常传递给UI层处理
            }
        }

        /// <summary>
        /// 更新区域信息
        /// </summary>
        public bool UpdateArea(Area area, User currentUser = null)
        {
            if (area == null) { throw new ArgumentNullException(nameof(area)); }
            if (string.IsNullOrWhiteSpace(area.AreaId)) { throw new ArgumentException("区域ID不能为空", nameof(area.AreaId)); }
            if (string.IsNullOrWhiteSpace(area.AreaName)) { throw new ArgumentException("区域名称不能为空", nameof(area.AreaName)); }
            // 确保不能将父区域设置为自身或其子区域（防止循环引用），这需要更复杂的逻辑

            try
            {
                bool success = areaDal.UpdateArea(area);
                // if (success && currentUser != null && systemLogBll != null)
                // {
                //     systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Update", "Area", $"区域 [{area.AreaName}] ({area.AreaId}) 更新成功");
                // }
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error updating area: " + ex.Message);
                throw;
            }
        }

        /// <summary>
        /// 删除区域（增加子区域检查）
        /// </summary>
        public bool DeleteArea(string areaId, User currentUser = null)
        {
            if (string.IsNullOrEmpty(areaId)) { throw new ArgumentNullException(nameof(areaId)); }

            try
            {
                // 业务逻辑：检查是否存在子区域
                if (areaDal.HasChildAreas(areaId))
                {
                    throw new InvalidOperationException("删除失败：该区域下存在子区域。");
                }
                
                // 获取区域名称用于日志 (可选，在删除前获取)
                string areaName = areaDal.GetAreaById(areaId)?.AreaName ?? areaId;

                bool success = areaDal.DeleteArea(areaId);
                // if (success && currentUser != null && systemLogBll != null)
                // {
                //     systemLogBll.AddLog(currentUser.Id, currentUser.Username, "Delete", "Area", $"区域 [{areaName}] ({areaId}) 删除成功");
                // }
                return success;
            }
            catch (InvalidOperationException ex) // 捕获业务逻辑异常
            {
                 Console.WriteLine("BLL Error deleting area: " + ex.Message);
                 throw;
            }
            catch (Exception ex) // 捕获DAL层可能抛出的外键约束等异常
            {
                Console.WriteLine("BLL Error deleting area: " + ex.Message);
                throw;
            }
        }
        
        /// <summary>
        /// 获取用于下拉框的区域列表 (ID 和 Name)
        /// </summary>
        public List<ComboboxItem> GetAreasForComboBox()
        {
             try
            {
                var areas = areaDal.GetAllAreas();
                // 转换成 ComboboxItem 列表
                return areas.Select(a => new ComboboxItem(a.AreaName, a.AreaId)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BLL Error getting areas for combobox: " + ex.Message);
                throw;
            }
        }
    }
} 