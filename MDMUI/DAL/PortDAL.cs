using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using MDMUI.Model;
using MDMUI.Utility;
using System.Diagnostics;

namespace MDMUI.DAL
{
    /// <summary>
    /// 端口数据访问类
    /// </summary>
    public class PortDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 根据子设备ID获取端口列表
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>包含端口信息的DataTable</returns>
        public DataTable GetPortsByParentDeviceId(string subDeviceId)
        {
            Debug.WriteLine($"开始查询子设备[{subDeviceId}]的端口信息");
            DataTable dt = new DataTable();

            try
            {
                if (string.IsNullOrEmpty(subDeviceId))
                {
                    throw new ArgumentException("子设备ID不能为空", nameof(subDeviceId));
                }
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT p.*, sd.sub_device_name as SubDeviceName
                        FROM port p
                        LEFT JOIN sub_device sd ON p.parent_device_id = sd.sub_device_id
                        WHERE p.parent_device_id = @subDeviceId
                        ORDER BY p.port_id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@subDeviceId", subDeviceId);

                        // 添加重试逻辑
                        int retryCount = 0;
                        int maxRetries = 3;
                        bool success = false;

                        while (!success && retryCount < maxRetries)
                        {
                            try
                            {
                                connection.Open();
                                Debug.WriteLine($"数据库连接已打开，开始执行查询端口");
                                
                                SqlDataAdapter adapter = new SqlDataAdapter(command);
                                adapter.Fill(dt);
                                success = true;
                                
                                Debug.WriteLine($"端口查询成功，返回 {dt.Rows.Count} 条记录");
                            }
                            catch (SqlException sqlEx)
                            {
                                retryCount++;
                                Debug.WriteLine($"端口SQL异常: {sqlEx.Message}, 错误码: {sqlEx.Number}, 重试次数: {retryCount}");
                                
                                if (retryCount >= maxRetries)
                                    throw;
                                
                                System.Threading.Thread.Sleep(1000 * retryCount); // 递增延迟重试
                            }
                            finally
                            {
                                if (connection.State == ConnectionState.Open)
                                    connection.Close();
                            }
                        }
                    }
                }

                // 检查表是否包含必要的列
                if (dt.Rows.Count > 0)
                {
                    Debug.WriteLine("验证返回的端口数据表结构");
                    string[] expectedColumns = new string[] { 
                        "port_id", "port_name", "port_type", "port_number", 
                        "protocol", "parent_device_id", "create_time", "create_user", 
                        "edit_time", "edit_user"
                    };
                    
                    foreach (string column in expectedColumns)
                    {
                        if (!dt.Columns.Contains(column))
                        {
                            Debug.WriteLine($"警告: 返回的端口数据表缺少列 '{column}'");
                            // 如果列缺失，添加空列以防止后续处理出错
                            dt.Columns.Add(column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取端口列表异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
                
                // 创建一个空表返回，防止空引用异常
                dt = new DataTable();
                string[] portColumns = { 
                    "port_id", "port_name", "port_type", "port_number", 
                    "protocol", "parent_device_id", "create_time", "create_user", 
                    "edit_time", "edit_user", "SubDeviceName" 
                };
                
                foreach (string column in portColumns)
                {
                    dt.Columns.Add(column);
                }
                
                Debug.WriteLine("已创建空端口数据表以防止后续处理出错");
                
                throw; // 重新抛出异常，以便上层捕获
            }

            return dt;
        }

        /// <summary>
        /// 获取所有端口列表
        /// </summary>
        /// <returns>包含端口信息的DataTable</returns>
        public DataTable GetAllPorts()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT p.*, sd.sub_device_name as SubDeviceName
                    FROM port p
                    LEFT JOIN sub_device sd ON p.sub_device_id = sd.sub_device_id
                    ORDER BY p.port_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 根据ID获取端口信息
        /// </summary>
        /// <param name="portId">端口ID</param>
        /// <returns>包含端口信息的DataTable</returns>
        public DataTable GetPortById(string portId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT p.*, sd.sub_device_name as SubDeviceName
                    FROM port p
                    LEFT JOIN sub_device sd ON p.parent_device_id = sd.sub_device_id
                    WHERE p.port_id = @portId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@portId", portId);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 添加端口
        /// </summary>
        /// <param name="port">端口对象</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool AddPort(Port port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO port 
                    (port_id, port_name, port_type, port_number, protocol,
                     parent_device_id, port_description, status, address, config, 
                     event_user, event_remark, create_time, event_type, create_user)
                    VALUES 
                    (@portId, @portName, @portType, @portNumber, @protocol,
                     @parentDeviceId, @portDescription, @status, @address, @config, 
                     @eventUser, @eventRemark, @createTime, @eventType, @createUser)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@portId", port.PortId);
                    command.Parameters.AddWithValue("@portName", port.PortName);
                    command.Parameters.AddWithValue("@portType", port.PortType);
                    command.Parameters.AddWithValue("@portNumber", port.PortNumber ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@protocol", port.Protocol ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@parentDeviceId", port.ParentDeviceId);
                    command.Parameters.AddWithValue("@portDescription", port.PortDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@status", port.Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@address", port.Address ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@config", port.Config ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventUser", port.EventUser ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventRemark", port.EventRemark ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@createTime", port.CreateTime);
                    command.Parameters.AddWithValue("@eventType", "Create");
                    command.Parameters.AddWithValue("@createUser", port.CreateUser ?? (object)DBNull.Value);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// 获取子设备最大端口编号
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>最大端口编号</returns>
        public int GetMaxPortNumberForDevice(string subDeviceId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT MAX(CAST(
                        CASE 
                            WHEN port_number LIKE '%[0-9]%' 
                            THEN SUBSTRING(port_number, PATINDEX('%[0-9]%', port_number), LEN(port_number)) 
                            ELSE '0' 
                        END AS INT))
                    FROM port 
                    WHERE parent_device_id = @subDeviceId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDeviceId);

                    connection.Open();
                    object result = command.ExecuteScalar();
                    
                    if (result == DBNull.Value || result == null)
                        return 0;
                        
                    int maxNumber;
                    if (int.TryParse(result.ToString(), out maxNumber))
                        return maxNumber;
                    else
                        return 0;
                }
            }
        }

        /// <summary>
        /// 更新端口信息
        /// </summary>
        /// <param name="port">端口对象</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdatePort(Port port)
        {
            if (port == null)
                throw new ArgumentNullException(nameof(port));

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE port SET 
                    port_name = @portName, 
                    port_type = @portType, 
                    port_description = @portDescription, 
                    sub_device_id = @subDeviceId, 
                    status = @status, 
                    address = @address, 
                    config = @config, 
                    event_user = @eventUser, 
                    event_remark = @eventRemark, 
                    edit_time = @editTime, 
                    event_type = @eventType
                    WHERE port_id = @portId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@portId", port.PortId);
                    command.Parameters.AddWithValue("@portName", port.PortName);
                    command.Parameters.AddWithValue("@portType", port.PortType);
                    command.Parameters.AddWithValue("@portDescription", port.PortDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@subDeviceId", port.SubDeviceId);
                    command.Parameters.AddWithValue("@status", port.Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@address", port.Address ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@config", port.Config ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventUser", port.EventUser ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventRemark", port.EventRemark ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@editTime", DateTime.Now);
                    command.Parameters.AddWithValue("@eventType", "Update");

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// 删除端口
        /// </summary>
        /// <param name="portId">端口ID</param>
        /// <param name="username">操作用户名</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeletePort(string portId, string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE port SET
                    event_user = @eventUser,
                    edit_time = @editTime,
                    event_type = @eventType
                    WHERE port_id = @portId;

                    DELETE FROM port WHERE port_id = @portId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@portId", portId);
                    command.Parameters.AddWithValue("@eventUser", username);
                    command.Parameters.AddWithValue("@editTime", DateTime.Now);
                    command.Parameters.AddWithValue("@eventType", "Delete");

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// (示例) 将 SqlDataReader 的一行映射到 Port 对象
        /// </summary>
        private Port MapReaderToPort(SqlDataReader reader)
        {
            return new Port
            {
                PortId = reader["port_id"].ToString(),
                PortName = reader["port_name"] != DBNull.Value ? reader["port_name"].ToString() : null,
                PortType = reader["port_type"] != DBNull.Value ? reader["port_type"].ToString() : null,
                // 处理可能的 DBNull 和类型转换
                Address = reader["address"] != DBNull.Value ? reader["address"].ToString() : null,
                Config = reader["config"] != DBNull.Value ? reader["config"].ToString() : null,
                SubDeviceId = reader["sub_device_id"].ToString()
                // ... 映射其他属性
            };
        }
    }
} 