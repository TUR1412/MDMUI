using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MDMUI.Utility
{
    /// <summary>
    /// 工艺流程顺序优化器，用于自动优化工艺站的顺序
    /// </summary>
    public class ProcessSequenceOptimizer
    {
        // 工站类型优先级（数值越低优先级越高）
        private Dictionary<string, int> stationTypePriority;
        
        // 工站依赖关系
        private Dictionary<string, List<string>> stationDependencies;
        
        /// <summary>
        /// 初始化工艺流程顺序优化器
        /// </summary>
        public ProcessSequenceOptimizer()
        {
            // 初始化工站类型优先级
            InitializeStationTypePriority();
            
            // 初始化为空依赖关系
            stationDependencies = new Dictionary<string, List<string>>();
        }
        
        /// <summary>
        /// 初始化工站类型优先级
        /// </summary>
        private void InitializeStationTypePriority()
        {
            stationTypePriority = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                { "Preparation", 10 },   // 准备工站
                { "Material", 20 },      // 物料处理
                { "Assembly", 30 },      // 装配工站
                { "Testing", 40 },       // 测试工站
                { "Inspection", 50 },    // 检验工站
                { "Packaging", 60 },     // 包装工站
                { "Shipping", 70 },      // 发货工站
                { "Standard", 100 },     // 标准工站
                { "Special", 200 },      // 特殊工站
                { "Other", 999 }         // 其他工站
            };
        }
        
        /// <summary>
        /// 设置工站依赖关系
        /// </summary>
        /// <param name="dependencies">依赖关系字典，键为工站ID，值为该工站依赖的前置工站ID列表</param>
        public void SetStationDependencies(Dictionary<string, List<string>> dependencies)
        {
            stationDependencies = dependencies ?? new Dictionary<string, List<string>>();
        }
        
        /// <summary>
        /// 添加工站依赖关系
        /// </summary>
        /// <param name="stationId">工站ID</param>
        /// <param name="dependsOnStationIds">该工站依赖的前置工站ID列表</param>
        public void AddStationDependency(string stationId, List<string> dependsOnStationIds)
        {
            if (string.IsNullOrEmpty(stationId)) return;
            
            if (!stationDependencies.ContainsKey(stationId))
            {
                stationDependencies[stationId] = new List<string>();
            }
            
            if (dependsOnStationIds != null)
            {
                foreach (string depId in dependsOnStationIds)
                {
                    if (!string.IsNullOrEmpty(depId) && !stationDependencies[stationId].Contains(depId))
                    {
                        stationDependencies[stationId].Add(depId);
                    }
                }
            }
        }
        
        /// <summary>
        /// 优化工艺站顺序
        /// </summary>
        /// <param name="routes">工艺路线数据表</param>
        /// <returns>优化后的工艺路线数据表</returns>
        public DataTable OptimizeSequence(DataTable routes)
        {
            if (routes == null || routes.Rows.Count == 0)
            {
                return routes;
            }
            
            // 创建包含所有必要信息的工站列表
            List<StationInfo> stations = new List<StationInfo>();
            
            // 从数据表中提取工站信息
            foreach (DataRow row in routes.Rows)
            {
                string stationId = row["StationId"].ToString();
                string stationType = row["StationType"].ToString();
                int currentSequence = Convert.ToInt32(row["Sequence"]);
                string routeId = row["RouteId"].ToString();
                
                StationInfo station = new StationInfo
                {
                    StationId = stationId,
                    StationType = stationType,
                    CurrentSequence = currentSequence,
                    RouteId = routeId,
                    Row = row
                };
                
                stations.Add(station);
            }
            
            // 执行排序
            IEnumerable<StationInfo> sortedStations = SortStations(stations);
            
            // 创建结果表
            DataTable result = routes.Clone();
            
            // 填充结果表，并更新序号
            int sequence = 1;
            foreach (StationInfo station in sortedStations)
            {
                DataRow newRow = result.NewRow();
                
                // 复制原始行的所有值
                foreach (DataColumn col in routes.Columns)
                {
                    newRow[col.ColumnName] = station.Row[col.ColumnName];
                }
                
                // 更新序号
                newRow["Sequence"] = sequence++;
                
                result.Rows.Add(newRow);
            }
            
            return result;
        }
        
        /// <summary>
        /// 排序工站列表
        /// </summary>
        /// <param name="stations">工站信息列表</param>
        /// <returns>排序后的工站列表</returns>
        private IEnumerable<StationInfo> SortStations(List<StationInfo> stations)
        {
            // 检查是否有依赖关系
            bool hasDependencies = stationDependencies.Count > 0;
            
            if (hasDependencies)
            {
                // 如果有依赖关系，先使用拓扑排序
                return TopologicalSort(stations);
            }
            else
            {
                // 如果没有依赖关系，使用工站类型优先级排序
                return stations.OrderBy(s => GetStationTypePriority(s.StationType))
                               .ThenBy(s => s.CurrentSequence);
            }
        }
        
        /// <summary>
        /// 执行拓扑排序（考虑工站依赖关系）
        /// </summary>
        /// <param name="stations">工站信息列表</param>
        /// <returns>拓扑排序后的工站列表</returns>
        private IEnumerable<StationInfo> TopologicalSort(List<StationInfo> stations)
        {
            // 创建工站ID到工站信息的映射
            Dictionary<string, StationInfo> stationMap = stations.ToDictionary(s => s.StationId);
            
            // 创建邻接表表示工站依赖关系图
            Dictionary<string, List<string>> graph = new Dictionary<string, List<string>>();
            
            // 初始化图
            foreach (StationInfo station in stations)
            {
                if (!graph.ContainsKey(station.StationId))
                {
                    graph[station.StationId] = new List<string>();
                }
            }
            
            // 填充图的边（依赖关系）
            foreach (var kvp in stationDependencies)
            {
                string stationId = kvp.Key;
                List<string> dependencies = kvp.Value;
                
                if (stationMap.ContainsKey(stationId))
                {
                    foreach (string depId in dependencies)
                    {
                        if (stationMap.ContainsKey(depId))
                        {
                            // 添加边：depId -> stationId（表示stationId依赖depId）
                            if (!graph.ContainsKey(depId))
                            {
                                graph[depId] = new List<string>();
                            }
                            
                            graph[depId].Add(stationId);
                        }
                    }
                }
            }
            
            // 计算每个工站的入度（有多少工站依赖它）
            Dictionary<string, int> inDegree = new Dictionary<string, int>();
            foreach (StationInfo station in stations)
            {
                inDegree[station.StationId] = 0;
            }
            
            foreach (var kvp in graph)
            {
                foreach (string target in kvp.Value)
                {
                    inDegree[target]++;
                }
            }
            
            // 使用队列进行拓扑排序
            Queue<string> queue = new Queue<string>();
            
            // 将入度为0的工站加入队列
            foreach (var kvp in inDegree)
            {
                if (kvp.Value == 0)
                {
                    queue.Enqueue(kvp.Key);
                }
            }
            
            // 存储排序结果
            List<StationInfo> sortedResult = new List<StationInfo>();
            
            // 执行拓扑排序
            while (queue.Count > 0)
            {
                string current = queue.Dequeue();
                
                if (stationMap.ContainsKey(current))
                {
                    sortedResult.Add(stationMap[current]);
                }
                
                if (graph.ContainsKey(current))
                {
                    foreach (string neighbor in graph[current])
                    {
                        inDegree[neighbor]--;
                        
                        if (inDegree[neighbor] == 0)
                        {
                            queue.Enqueue(neighbor);
                        }
                    }
                }
            }
            
            // 检查是否有循环依赖（如果sortedResult.Count < stations.Count，说明有循环）
            if (sortedResult.Count < stations.Count)
            {
                // 有循环依赖，使用备用排序方法
                var remainingStations = stations.Except(sortedResult).ToList();
                
                // 对剩余工站使用类型优先级排序
                var sortedRemaining = remainingStations
                    .OrderBy(s => GetStationTypePriority(s.StationType))
                    .ThenBy(s => s.CurrentSequence);
                
                // 将排序后的剩余工站加入结果
                sortedResult.AddRange(sortedRemaining);
            }
            
            return sortedResult;
        }
        
        /// <summary>
        /// 获取工站类型的优先级
        /// </summary>
        /// <param name="stationType">工站类型</param>
        /// <returns>优先级值（值越小优先级越高）</returns>
        private int GetStationTypePriority(string stationType)
        {
            if (string.IsNullOrEmpty(stationType))
            {
                return int.MaxValue;
            }
            
            if (stationTypePriority.TryGetValue(stationType, out int priority))
            {
                return priority;
            }
            
            return stationTypePriority["Other"]; // 默认为其他类型的优先级
        }
        
        /// <summary>
        /// 验证工艺路线顺序
        /// </summary>
        /// <param name="routes">工艺路线数据表</param>
        /// <returns>验证结果，包括问题列表</returns>
        public SequenceValidationResult ValidateSequence(DataTable routes)
        {
            SequenceValidationResult result = new SequenceValidationResult();
            
            if (routes == null || routes.Rows.Count == 0)
            {
                return result;
            }
            
            // 验证序号是否连续
            ValidateSequenceNumbers(routes, result);
            
            // 验证工站类型顺序
            ValidateStationTypeOrder(routes, result);
            
            // 验证依赖关系
            ValidateDependencies(routes, result);
            
            return result;
        }
        
        /// <summary>
        /// 验证序号是否连续
        /// </summary>
        private void ValidateSequenceNumbers(DataTable routes, SequenceValidationResult result)
        {
            // 获取所有序号
            List<int> sequences = new List<int>();
            foreach (DataRow row in routes.Rows)
            {
                sequences.Add(Convert.ToInt32(row["Sequence"]));
            }
            
            // 排序
            sequences.Sort();
            
            // 检查序号是否连续
            for (int i = 0; i < sequences.Count; i++)
            {
                if (sequences[i] != i + 1)
                {
                    result.Issues.Add(new SequenceIssue
                    {
                        IssueType = SequenceIssueType.NonConsecutiveSequence,
                        Description = $"序号不连续：预期 {i + 1}，实际为 {sequences[i]}"
                    });
                    break;
                }
            }
        }
        
        /// <summary>
        /// 验证工站类型顺序
        /// </summary>
        private void ValidateStationTypeOrder(DataTable routes, SequenceValidationResult result)
        {
            string previousType = null;
            int previousPriority = -1;
            int previousSequence = -1;
            
            foreach (DataRow row in routes.Rows)
            {
                string currentType = row["StationType"].ToString();
                int currentSequence = Convert.ToInt32(row["Sequence"]);
                int currentPriority = GetStationTypePriority(currentType);
                
                if (previousType != null)
                {
                    // 如果当前类型优先级比前一个低，但却排在前一个之前，可能是不合理的顺序
                    if (currentPriority < previousPriority && currentSequence > previousSequence)
                    {
                        result.Issues.Add(new SequenceIssue
                        {
                            IssueType = SequenceIssueType.StationTypeOrderWarning,
                            Description = $"工站类型顺序可能不合理：{currentType}（序号 {currentSequence}）通常应该在 {previousType}（序号 {previousSequence}）之前"
                        });
                    }
                }
                
                previousType = currentType;
                previousPriority = currentPriority;
                previousSequence = currentSequence;
            }
        }
        
        /// <summary>
        /// 验证依赖关系
        /// </summary>
        private void ValidateDependencies(DataTable routes, SequenceValidationResult result)
        {
            if (stationDependencies.Count == 0)
            {
                return;
            }
            
            // 创建工站ID到序号的映射
            Dictionary<string, int> stationSequence = new Dictionary<string, int>();
            
            foreach (DataRow row in routes.Rows)
            {
                string stationId = row["StationId"].ToString();
                int sequence = Convert.ToInt32(row["Sequence"]);
                stationSequence[stationId] = sequence;
            }
            
            // 检查每个依赖关系
            foreach (var kvp in stationDependencies)
            {
                string stationId = kvp.Key;
                List<string> dependencies = kvp.Value;
                
                if (stationSequence.ContainsKey(stationId))
                {
                    int currentSequence = stationSequence[stationId];
                    
                    foreach (string depId in dependencies)
                    {
                        if (stationSequence.ContainsKey(depId))
                        {
                            int depSequence = stationSequence[depId];
                            
                            // 如果依赖的工站排在当前工站后面，这是一个依赖违反
                            if (depSequence >= currentSequence)
                            {
                                result.Issues.Add(new SequenceIssue
                                {
                                    IssueType = SequenceIssueType.DependencyViolation,
                                    Description = $"依赖关系违反：工站 {stationId}（序号 {currentSequence}）依赖于工站 {depId}（序号 {depSequence}），但是排在其前面"
                                });
                            }
                        }
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 工站信息类，用于排序过程中
    /// </summary>
    internal class StationInfo
    {
        /// <summary>
        /// 工站ID
        /// </summary>
        public string StationId { get; set; }
        
        /// <summary>
        /// 工站类型
        /// </summary>
        public string StationType { get; set; }
        
        /// <summary>
        /// 当前序号
        /// </summary>
        public int CurrentSequence { get; set; }
        
        /// <summary>
        /// 路由ID
        /// </summary>
        public string RouteId { get; set; }
        
        /// <summary>
        /// 对应的数据行
        /// </summary>
        public DataRow Row { get; set; }
    }
    
    /// <summary>
    /// 序列验证结果类
    /// </summary>
    public class SequenceValidationResult
    {
        /// <summary>
        /// 问题列表
        /// </summary>
        public List<SequenceIssue> Issues { get; private set; }
        
        /// <summary>
        /// 是否有严重问题
        /// </summary>
        public bool HasCriticalIssues
        {
            get { return Issues.Any(i => i.IssueType == SequenceIssueType.DependencyViolation || i.IssueType == SequenceIssueType.NonConsecutiveSequence); }
        }
        
        /// <summary>
        /// 是否有任何问题
        /// </summary>
        public bool HasIssues
        {
            get { return Issues.Count > 0; }
        }
        
        public SequenceValidationResult()
        {
            Issues = new List<SequenceIssue>();
        }
    }
    
    /// <summary>
    /// 序列问题类型枚举
    /// </summary>
    public enum SequenceIssueType
    {
        /// <summary>
        /// 序号不连续
        /// </summary>
        NonConsecutiveSequence,
        
        /// <summary>
        /// 工站类型顺序不合理（警告级别）
        /// </summary>
        StationTypeOrderWarning,
        
        /// <summary>
        /// 依赖关系违反
        /// </summary>
        DependencyViolation
    }
    
    /// <summary>
    /// 序列问题类
    /// </summary>
    public class SequenceIssue
    {
        /// <summary>
        /// 问题类型
        /// </summary>
        public SequenceIssueType IssueType { get; set; }
        
        /// <summary>
        /// 问题描述
        /// </summary>
        public string Description { get; set; }
    }
} 