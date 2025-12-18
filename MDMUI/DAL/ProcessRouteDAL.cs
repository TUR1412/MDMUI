using MDMUI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace MDMUI.DAL
{
    public class ProcessRouteDAL
    {
        private readonly string connectionString;

        public ProcessRouteDAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        /// <summary>
        /// 根据工艺流程ID获取工艺路线
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>工艺路线数据表</returns>
        public DataTable GetRoutesByProcessId(string processId)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT RouteId, StationId, Version, ProcessId, 
                               Description, Sequence, StationType, 
                               CreateTime, Status
                        FROM ProcessRoute
                        WHERE ProcessId = @ProcessId
                        ORDER BY Sequence";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProcessId", processId);
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"根据工艺流程ID获取工艺路线数据失败: {ex.Message}");
                throw;
            }
            
            return dt;
        }
        
        /// <summary>
        /// 获取不在当前工艺流程中的工艺站列表
        /// </summary>
        /// <param name="processId">工艺流程ID</param>
        /// <returns>不在当前工艺流程中的工艺站列表</returns>
        public DataTable GetNonOperListByFid(string processId)
        {
            DataTable dt = new DataTable();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT DISTINCT StationId as id, 
                               StationId as oper_id, 
                               Version as oper_version, 
                               Description as oper_description, 
                               StationType as oper_type
                        FROM ProcessRoute
                        WHERE StationId NOT IN (
                            SELECT StationId FROM ProcessRoute WHERE ProcessId = @ProcessId
                        )";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ProcessId", processId);
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"获取不在当前工艺流程中的工艺站列表失败: {ex.Message}");
                throw;
            }
            
            return dt;
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        UPDATE ProcessRoute
                        SET Sequence = @NewSequence
                        WHERE RouteId = @RouteId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", routeId);
                        command.Parameters.AddWithValue("@NewSequence", newSequence);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"更新工艺路线顺序失败: {ex.Message}");
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
                // 首先获取工艺站的详细信息
                DataTable stationInfo = new DataTable();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string queryInfo = @"
                        SELECT TOP 1 Version, Description, StationType
                        FROM ProcessRoute
                        WHERE StationId = @StationId";
                    
                    using (SqlCommand command = new SqlCommand(queryInfo, connection))
                    {
                        command.Parameters.AddWithValue("@StationId", operId);
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(stationInfo);
                        }
                        
                        // 如果没有找到工艺站信息，使用默认值
                        string version = "V1.0";
                        string description = "新工艺站";
                        string stationType = "Standard";
                        
                        if (stationInfo.Rows.Count > 0)
                        {
                            version = stationInfo.Rows[0]["Version"].ToString();
                            description = stationInfo.Rows[0]["Description"].ToString();
                            stationType = stationInfo.Rows[0]["StationType"].ToString();
                        }
                        
                        // 插入到ProcessRoute表
                        string query = @"
                            INSERT INTO ProcessRoute (RouteId, StationId, Version, ProcessId, Description, Sequence, StationType, CreateTime, Status)
                            VALUES (NEWID(), @StationId, @Version, @ProcessId, @Description, @Sequence, @StationType, GETDATE(), 'Active')";
                        
                        using (SqlCommand insertCommand = new SqlCommand(query, connection))
                        {
                            insertCommand.Parameters.AddWithValue("@StationId", operId);
                            insertCommand.Parameters.AddWithValue("@Version", version);
                            insertCommand.Parameters.AddWithValue("@ProcessId", processId);
                            insertCommand.Parameters.AddWithValue("@Description", description);
                            insertCommand.Parameters.AddWithValue("@Sequence", sequence);
                            insertCommand.Parameters.AddWithValue("@StationType", stationType);
                            
                            int result = insertCommand.ExecuteNonQuery();
                            return result > 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"添加工艺站到工艺流程失败: {ex.Message}");
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
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        DELETE FROM ProcessRoute
                        WHERE RouteId = @RouteId";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@RouteId", routeId);
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        return result > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine($"从工艺流程中移除工艺站失败: {ex.Message}");
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
            if (updates == null || updates.Count == 0)
                return true;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // 使用事务确保原子性
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            string query = @"
                                UPDATE ProcessRoute
                                SET Sequence = @NewSequence
                                WHERE RouteId = @RouteId";
                            
                            foreach (var update in updates)
                            {
                                using (SqlCommand command = new SqlCommand(query, connection, transaction))
                                {
                                    command.Parameters.AddWithValue("@RouteId", update.Key);
                                    command.Parameters.AddWithValue("@NewSequence", update.Value);
                                    command.ExecuteNonQuery();
                                }
                            }
                            
                            // 提交事务
                            transaction.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            // 回滚事务
                            transaction.Rollback();
                            Console.WriteLine($"批量更新工艺路线序号失败: {ex.Message}");
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"批量更新工艺路线序号失败: {ex.Message}");
                throw;
            }
        }
        
        /// <summary>
        /// 获取工站依赖关系
        /// </summary>
        /// <returns>工站依赖关系字典，键为工站ID，值为依赖的工站ID列表</returns>
        public Dictionary<string, List<string>> GetStationDependencies()
        {
            Dictionary<string, List<string>> dependencies = new Dictionary<string, List<string>>();
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 这里使用一个示例查询，实际项目中需要根据实际数据库结构调整
                    // 假设有一个StationDependency表存储工站间的依赖关系
                    string query = @"
                        SELECT StationId, DependsOnStationId
                        FROM StationDependency";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string stationId = reader["StationId"].ToString();
                                string dependsOnId = reader["DependsOnStationId"].ToString();
                                
                                if (!dependencies.ContainsKey(stationId))
                                {
                                    dependencies[stationId] = new List<string>();
                                }
                                
                                dependencies[stationId].Add(dependsOnId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 在实际项目中，如果依赖关系表不存在，可以忽略此异常
                Console.WriteLine($"获取工站依赖关系失败（可能是表不存在）: {ex.Message}");
                // 返回空依赖关系，而不是抛出异常
            }
            
            return dependencies;
        }
        
        /// <summary>
        /// 获取工站类型的排序规则
        /// </summary>
        /// <returns>工站类型优先级字典，键为类型名称，值为优先级（数值越小优先级越高）</returns>
        public Dictionary<string, int> GetStationTypeRules()
        {
            Dictionary<string, int> rules = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // 这里使用一个示例查询，实际项目中需要根据实际数据库结构调整
                    // 假设有一个StationTypeRule表存储工站类型的排序规则
                    string query = @"
                        SELECT StationType, SortPriority
                        FROM StationTypeRule
                        ORDER BY SortPriority";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string stationType = reader["StationType"].ToString();
                                int priority = Convert.ToInt32(reader["SortPriority"]);
                                
                                rules[stationType] = priority;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // 在实际项目中，如果规则表不存在，可以忽略此异常
                Console.WriteLine($"获取工站类型排序规则失败（可能是表不存在）: {ex.Message}");
                // 返回默认规则，而不是抛出异常
                rules = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
                {
                    { "Preparation", 10 },
                    { "Material", 20 },
                    { "Assembly", 30 },
                    { "Testing", 40 },
                    { "Inspection", 50 },
                    { "Packaging", 60 },
                    { "Shipping", 70 },
                    { "Standard", 100 },
                    { "Special", 200 },
                    { "Other", 999 }
                };
            }
            
            return rules;
        }
    }
} 