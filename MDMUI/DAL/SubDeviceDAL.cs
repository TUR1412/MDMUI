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
    /// 子设备数据访问类
    /// </summary>
    public class SubDeviceDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        /// <summary>
        /// 根据设备组ID获取子设备列表
        /// </summary>
        /// <param name="eqpGroupId">设备组ID</param>
        /// <returns>包含子设备信息的DataTable</returns>
        public DataTable GetSubDevicesByGroupId(string eqpGroupId)
        {
            Debug.WriteLine($"开始查询设备组[{eqpGroupId}]的子设备信息");
            DataTable dt = new DataTable();

            try
            {
                if (string.IsNullOrEmpty(eqpGroupId))
                {
                    throw new ArgumentException("设备组ID不能为空", nameof(eqpGroupId));
                }
                
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
                        SELECT sd.*, eg.eqp_group_description as EqpGroupName
                        FROM sub_device sd
                        LEFT JOIN eqp_group eg ON sd.eqp_group_id = eg.eqp_group_id
                        WHERE sd.eqp_group_id = @eqpGroupId
                        ORDER BY sd.sub_device_id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@eqpGroupId", eqpGroupId);

                        // 添加重试逻辑
                        int retryCount = 0;
                        int maxRetries = 3;
                        bool success = false;

                        while (!success && retryCount < maxRetries)
                        {
                            try
                            {
                                connection.Open();
                                Debug.WriteLine($"数据库连接已打开，开始执行查询");
                                
                                SqlDataAdapter adapter = new SqlDataAdapter(command);
                                adapter.Fill(dt);
                                success = true;
                                
                                Debug.WriteLine($"查询成功，返回 {dt.Rows.Count} 条记录");
                            }
                            catch (SqlException sqlEx)
                            {
                                retryCount++;
                                Debug.WriteLine($"SQL异常: {sqlEx.Message}, 错误码: {sqlEx.Number}, 重试次数: {retryCount}");
                                
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
                    Debug.WriteLine("验证返回的数据表结构");
                    string[] expectedColumns = new string[] { 
                        "sub_device_id", "sub_device_name", "sub_device_type", "eqp_group_id",
                        "create_time", "create_user", "edit_time", "edit_user"
                    };
                    
                    foreach (string column in expectedColumns)
                    {
                        if (!dt.Columns.Contains(column))
                        {
                            Debug.WriteLine($"警告: 返回的数据表缺少列 '{column}'");
                            // 如果列缺失，添加空列以防止后续处理出错
                            dt.Columns.Add(column);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"获取子设备列表异常: {ex.GetType().Name}: {ex.Message}");
                Debug.WriteLine($"异常堆栈: {ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"内部异常: {ex.InnerException.Message}");
                }
                
                throw; // 重新抛出异常，以便上层捕获
            }

            return dt;
        }

        /// <summary>
        /// 获取所有子设备列表
        /// </summary>
        /// <returns>包含子设备信息的DataTable</returns>
        public DataTable GetAllSubDevices()
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT sd.*, eg.eqp_group_description as EqpGroupName
                    FROM sub_device sd
                    LEFT JOIN eqp_group eg ON sd.eqp_group_id = eg.eqp_group_id
                    ORDER BY sd.sub_device_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 根据ID获取子设备信息
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>子设备数据表</returns>
        public DataTable GetSubDeviceById(string subDeviceId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT sd.*, eg.eqp_group_description as EqpGroupName
                    FROM sub_device sd
                    LEFT JOIN eqp_group eg ON sd.eqp_group_id = eg.eqp_group_id
                    WHERE sd.sub_device_id = @subDeviceId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDeviceId);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 添加子设备
        /// </summary>
        /// <param name="subDevice">子设备对象</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool AddSubDevice(SubDevice subDevice)
        {
            if (subDevice == null)
                throw new ArgumentNullException(nameof(subDevice));

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO sub_device 
                    (sub_device_id, sub_device_name, sub_device_type, sub_device_description, 
                     eqp_group_id, status, event_user, event_remark, create_time, event_type,
                     create_user)
                    VALUES 
                    (@subDeviceId, @subDeviceName, @subDeviceType, @subDeviceDescription, 
                     @eqpGroupId, @status, @eventUser, @eventRemark, @createTime, @eventType,
                     @createUser)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDevice.SubDeviceId);
                    command.Parameters.AddWithValue("@subDeviceName", subDevice.SubDeviceName);
                    command.Parameters.AddWithValue("@subDeviceType", subDevice.SubDeviceType);
                    command.Parameters.AddWithValue("@subDeviceDescription", subDevice.SubDeviceDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eqpGroupId", subDevice.EqpGroupId);
                    command.Parameters.AddWithValue("@status", subDevice.Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventUser", subDevice.EventUser ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventRemark", subDevice.EventRemark ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@createTime", subDevice.CreateTime);
                    command.Parameters.AddWithValue("@eventType", "Create");
                    command.Parameters.AddWithValue("@createUser", subDevice.CreateUser ?? (object)DBNull.Value);

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// 获取子设备关联的端口
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <returns>包含端口信息的数据表</returns>
        public DataTable GetAssociatedPorts(string subDeviceId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    SELECT * FROM port 
                    WHERE sub_device_id = @subDeviceId 
                    ORDER BY port_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDeviceId);

                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 获取最大子设备ID
        /// </summary>
        /// <returns>最大子设备ID</returns>
        public string GetMaxSubDeviceId()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT MAX(sub_device_id) FROM sub_device WHERE sub_device_id LIKE 'SD%'";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result == DBNull.Value ? null : result.ToString();
                }
            }
        }

        /// <summary>
        /// 更新子设备信息
        /// </summary>
        /// <param name="subDevice">子设备对象</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdateSubDevice(SubDevice subDevice)
        {
            if (subDevice == null)
                throw new ArgumentNullException(nameof(subDevice));

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
                    UPDATE sub_device SET 
                    sub_device_name = @subDeviceName, 
                    sub_device_type = @subDeviceType, 
                    sub_device_description = @subDeviceDescription, 
                    eqp_group_id = @eqpGroupId, 
                    status = @status, 
                    event_user = @eventUser, 
                    event_remark = @eventRemark, 
                    edit_time = @editTime, 
                    event_type = @eventType
                    WHERE sub_device_id = @subDeviceId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDevice.SubDeviceId);
                    command.Parameters.AddWithValue("@subDeviceName", subDevice.SubDeviceName);
                    command.Parameters.AddWithValue("@subDeviceType", subDevice.SubDeviceType);
                    command.Parameters.AddWithValue("@subDeviceDescription", subDevice.SubDeviceDescription ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eqpGroupId", subDevice.EqpGroupId);
                    command.Parameters.AddWithValue("@status", subDevice.Status ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventUser", subDevice.EventUser ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@eventRemark", subDevice.EventRemark ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@editTime", DateTime.Now);
                    command.Parameters.AddWithValue("@eventType", "Update");

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }

        /// <summary>
        /// 删除子设备
        /// </summary>
        /// <param name="subDeviceId">子设备ID</param>
        /// <param name="username">操作用户名</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool DeleteSubDevice(string subDeviceId, string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 首先检查是否有关联的端口，如果有则不允许删除
                string checkQuery = "SELECT COUNT(*) FROM port WHERE sub_device_id = @subDeviceId";
                
                connection.Open();
                using (SqlCommand checkCommand = new SqlCommand(checkQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@subDeviceId", subDeviceId);
                    int count = (int)checkCommand.ExecuteScalar();
                    
                    if (count > 0)
                    {
                        throw new InvalidOperationException($"无法删除子设备 '{subDeviceId}'，因为它有 {count} 个关联的端口。请先删除这些端口。");
                    }
                }

                // 没有关联的端口，可以删除子设备
                string query = @"
                    UPDATE sub_device SET
                    event_user = @eventUser,
                    edit_time = @editTime,
                    event_type = @eventType
                    WHERE sub_device_id = @subDeviceId;

                    DELETE FROM sub_device WHERE sub_device_id = @subDeviceId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@subDeviceId", subDeviceId);
                    command.Parameters.AddWithValue("@eventUser", username);
                    command.Parameters.AddWithValue("@editTime", DateTime.Now);
                    command.Parameters.AddWithValue("@eventType", "Delete");

                    int result = command.ExecuteNonQuery();
                    return result > 0;
                }
            }
        }
    }
} 