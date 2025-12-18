using MDMUI.DAL;
using MDMUI.Model;
using MDMUI.Utility;
using System;
using System.Collections.Generic;
using System.Data;

namespace MDMUI.BLL
{
    public class ProcessRouteBLL
    {
        private readonly ProcessRouteDAL dal;
        private readonly ProcessSequenceOptimizer optimizer;

        public ProcessRouteBLL()
        {
            dal = new ProcessRouteDAL();
            optimizer = new ProcessSequenceOptimizer();
            
            // 初始化工站依赖关系
            InitializeStationDependencies();
        }

        /// <summary>
        /// 初始化工站依赖关系
        /// </summary>
        private void InitializeStationDependencies()
        {
            // 获取工站依赖关系
            Dictionary<string, List<string>> dependencies = dal.GetStationDependencies();
            optimizer.SetStationDependencies(dependencies);
        }

        /// <summary>
        /// 根据工艺流程ID获取工艺路线
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>工艺路线数据表</returns>
        public DataTable GetRoutesByProcessId(string processId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processId))
                {
                    throw new ArgumentException("工艺流程ID不能为空", nameof(processId));
                }
                
                return dal.GetRoutesByProcessId(processId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层根据工艺流程ID获取工艺路线失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 获取不在当前工艺流程中的工艺站列表
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>不在当前工艺流程中的工艺站列表</returns>
        public DataTable GetNonOperListByFid(string processId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processId))
                {
                    throw new ArgumentException("工艺流程ID不能为空", nameof(processId));
                }
                
                return dal.GetNonOperListByFid(processId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层获取不在当前工艺流程中的工艺站列表失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 更新工艺路线顺序
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <param name="newSequence">新的顺序</param>
        /// <returns>是否更新成功</returns>
        public bool UpdateRouteSequence(string routeId, int newSequence)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(routeId))
                {
                    throw new ArgumentException("工艺路线ID不能为空", nameof(routeId));
                }
                
                if (newSequence < 0)
                {
                    throw new ArgumentException("工艺站顺序不能小于0", nameof(newSequence));
                }
                
                return dal.UpdateRouteSequence(routeId, newSequence);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层更新工艺路线顺序失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 批量更新工艺路线序号
        /// </summary>
        /// <param name="updates">更新项列表，键为路由ID，值为新序号</param>
        /// <returns>是否全部更新成功</returns>
        public bool BatchUpdateRouteSequence(Dictionary<string, int> updates)
        {
            try
            {
                if (updates == null || updates.Count == 0)
                {
                    throw new ArgumentException("更新项列表不能为空", nameof(updates));
                }
                
                return dal.BatchUpdateRouteSequence(updates);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层批量更新工艺路线序号失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 添加工艺站到工艺流程中
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <param name="operId">工艺站ID</param>
        /// <param name="sequence">顺序</param>
        /// <returns>是否添加成功</returns>
        public bool AddOperToProcess(string processId, string operId, int sequence)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processId))
                {
                    throw new ArgumentException("工艺流程ID不能为空", nameof(processId));
                }
                
                if (string.IsNullOrWhiteSpace(operId))
                {
                    throw new ArgumentException("工艺站ID不能为空", nameof(operId));
                }
                
                if (sequence < 0)
                {
                    throw new ArgumentException("工艺站顺序不能小于0", nameof(sequence));
                }
                
                return dal.AddOperToProcess(processId, operId, sequence);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层添加工艺站到工艺流程失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 从工艺流程中移除工艺站
        /// </summary>
        /// <param name="routeId">工艺路线ID</param>
        /// <returns>是否移除成功</returns>
        public bool RemoveOperFromProcess(string routeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(routeId))
                {
                    throw new ArgumentException("工艺路线ID不能为空", nameof(routeId));
                }
                
                return dal.RemoveOperFromProcess(routeId);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层从工艺流程中移除工艺站失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 优化工艺站顺序
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>是否优化成功</returns>
        public bool OptimizeRouteSequence(string processId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processId))
                {
                    throw new ArgumentException("工艺流程ID不能为空", nameof(processId));
                }
                
                // 获取当前工艺流程的路线
                DataTable routes = dal.GetRoutesByProcessId(processId);
                
                if (routes == null || routes.Rows.Count <= 1)
                {
                    // 只有0或1个工站，无需优化
                    return true;
                }
                
                // 优化顺序
                DataTable optimizedRoutes = optimizer.OptimizeSequence(routes);
                
                // 准备批量更新
                Dictionary<string, int> updates = new Dictionary<string, int>();
                
                foreach (DataRow row in optimizedRoutes.Rows)
                {
                    string routeId = row["RouteId"].ToString();
                    int sequence = Convert.ToInt32(row["Sequence"]);
                    updates[routeId] = sequence;
                }
                
                // 执行批量更新
                return dal.BatchUpdateRouteSequence(updates);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层优化工艺站顺序失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 验证工艺路线顺序
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>验证结果</returns>
        public SequenceValidationResult ValidateRouteSequence(string processId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(processId))
                {
                    throw new ArgumentException("工艺流程ID不能为空", nameof(processId));
                }
                
                // 获取当前工艺流程的路线
                DataTable routes = dal.GetRoutesByProcessId(processId);
                
                if (routes == null || routes.Rows.Count <= 1)
                {
                    // 只有0或1个工站，无需验证
                    return new SequenceValidationResult();
                }
                
                // 验证顺序
                return optimizer.ValidateSequence(routes);
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"BLL层验证工艺路线顺序失败: {ex.Message}");
                throw;
            }
        }
    }
} 