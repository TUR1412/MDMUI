using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model; // Ensure Equipment model is referenced correctly
using MDMUI.Utility; // For DbConnectionHelper

namespace MDMUI.DAL
{
    // 为SqlDataReader添加扩展方法的静态类
    public static class SqlDataReaderExtensions
    {
        // 辅助扩展方法用于安全检查列是否存在
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }

    public class EquipmentDAL
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        /// <summary>
        /// Gets a list of equipment suitable for a ComboBox (Id and Name/Description).
        /// Uses EquipmentId and EquipmentDescription based on the updated model and UI usage.
        /// </summary>
        /// <returns>A list of ComboboxItem objects representing equipment.</returns>
        public List<ComboboxItem> GetAllEquipmentForComboBox()
        {
            List<ComboboxItem> equipmentList = new List<ComboboxItem>();
            string query = "SELECT EquipmentId, Description FROM Equipment ORDER BY Description;"; 
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipmentList.Add(new ComboboxItem(
                                reader["Description"]?.ToString() ?? string.Empty, // Text from DB 'Description' column
                                reader["EquipmentId"] // Value
                            ));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching equipment data for ComboBox: " + ex.Message); 
                    // Log exception properly
                }
            }
            return equipmentList;
        }

        /// <summary>
        /// Gets a list of all equipment with detailed information, including joined data.
        /// Updated to reflect the new Equipment model properties.
        /// </summary>
        /// <returns>DataTable containing all equipment records.</returns>
        public DataTable GetAllEquipment()
        {
            DataTable dataTable = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 使用正确的列名，特别是Description而不是EquipmentDescription
                string query = @"SELECT 
                                    e.EquipmentId, 
                                    e.EquipmentName, 
                                    e.Description, 
                                    e.CategoryId,
                                    ec.CategoryName,
                                    e.FactoryId,
                                    f.FactoryName,
                                    e.Model, 
                                    e.Manufacturer,
                                    e.PurchaseDate,
                                    e.PurchasePrice,
                                    e.Status,
                                    e.Location,
                                    e.ResponsiblePerson,
                                    emp.EmployeeName as ResponsiblePersonName,
                                    e.MaintenanceCycle,
                                    e.LastMaintenanceDate,
                                    e.NextMaintenanceDate,
                                    e.CreateTime
                                 FROM Equipment e 
                                 LEFT JOIN EquipmentCategory ec ON e.CategoryId = ec.CategoryId
                                 LEFT JOIN Factory f ON e.FactoryId = f.FactoryId
                                 LEFT JOIN Employee emp ON e.ResponsiblePerson = emp.EmployeeId
                                 ORDER BY e.EquipmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error fetching equipment data: " + ex.Message);
                        // 记录错误，但返回空表而不是抛出异常，让UI能优雅处理
                    }
                }
            }
            return dataTable;
        }
        
        /// <summary>
        /// Gets detailed information for a specific equipment by ID.
        /// Returns an Equipment object mapped from the DataRow.
        /// </summary>
        /// <param name="equipmentId">The ID of the equipment to retrieve.</param>
        /// <returns>An Equipment object or null if not found.</returns>
        public Equipment GetEquipmentById(string equipmentId)
        {
            Equipment equipment = null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // 确保使用正确的表关联和列名
                string query = @"SELECT 
                                    e.EquipmentId, 
                                    e.EquipmentName, 
                                    e.Description, 
                                    e.CategoryId,
                                    ec.CategoryName,
                                    e.FactoryId,
                                    f.FactoryName,
                                    e.Model, 
                                    e.Manufacturer,
                                    e.PurchaseDate,
                                    e.PurchasePrice,
                                    e.Status,
                                    e.Location,
                                    e.ResponsiblePerson,
                                    emp.EmployeeName as ResponsiblePersonName,
                                    e.MaintenanceCycle,
                                    e.LastMaintenanceDate,
                                    e.NextMaintenanceDate,
                                    e.CreateTime
                                 FROM Equipment e 
                                 LEFT JOIN EquipmentCategory ec ON e.CategoryId = ec.CategoryId
                                 LEFT JOIN Factory f ON e.FactoryId = f.FactoryId
                                 LEFT JOIN Employee emp ON e.ResponsiblePerson = emp.EmployeeId
                                 WHERE e.EquipmentId = @EquipmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);
                    
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                equipment = MapReaderToEquipment(reader);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error fetching equipment with ID {equipmentId}: {ex.Message}");
                        // 记录错误但返回null，让UI能优雅处理
                    }
                }
            }
            return equipment;
        }
        
        /// <summary>
        /// Inserts a new equipment record into the database using a strongly-typed model.
        /// </summary>
        /// <param name="equipment">The Equipment object to insert.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public bool InsertEquipment(Equipment equipment)
        {
            // Corrected INSERT statement based on actual 'Equipment' table columns
            string query = @"INSERT INTO Equipment (
                                    EquipmentId, EquipmentName, CategoryId, FactoryId, Model, Manufacturer, 
                                    PurchaseDate, PurchasePrice, Status, Location, ResponsiblePerson, 
                                    MaintenanceCycle, LastMaintenanceDate, NextMaintenanceDate, Description, CreateTime
                                    -- Removed: EquipmentType, EquipmentSubType, EqpGroupId, EquipmentLayer, EventUser, EventRemark, EditTime
                                ) 
                                VALUES (
                                    @EquipmentId, @EquipmentName, @CategoryId, @FactoryId, @Model, @Manufacturer,
                                    @PurchaseDate, @PurchasePrice, @Status, @Location, @ResponsiblePerson,
                                    @MaintenanceCycle, @LastMaintenanceDate, @NextMaintenanceDate, @Description, @CreateTime
                                );";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                command.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName); // Directly from model
                command.Parameters.AddWithValue("@CategoryId", equipment.CategoryId); // Assuming model has CategoryId
                command.Parameters.AddWithValue("@FactoryId", (object)equipment.FactoryId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Model", (object)equipment.Model ?? DBNull.Value);
                command.Parameters.AddWithValue("@Manufacturer", (object)equipment.Manufacturer ?? DBNull.Value);
                command.Parameters.AddWithValue("@PurchaseDate", (object)equipment.PurchaseDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@PurchasePrice", (object)equipment.PurchasePrice ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", equipment.Status ?? "正常");
                command.Parameters.AddWithValue("@Location", (object)equipment.Location ?? DBNull.Value);
                command.Parameters.AddWithValue("@ResponsiblePerson", (object)equipment.ResponsiblePerson ?? DBNull.Value);
                command.Parameters.AddWithValue("@MaintenanceCycle", (object)equipment.MaintenanceCycle ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastMaintenanceDate", (object)equipment.LastMaintenanceDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@NextMaintenanceDate", (object)equipment.NextMaintenanceDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value); // 修正：使用Description属性
                command.Parameters.AddWithValue("@CreateTime", equipment.CreateTime); 
                
                // Parameters for columns not in Equipment table are removed:
                // @EquipmentType, @EquipmentSubType, @EqpGroupId, @EquipmentLayer, @EventUser, @EventRemark, @EditTime

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting equipment data: " + ex.Message);
                    return false; // Consider re-throwing or custom exception
                }
            }
        }
        
        /// <summary>
        /// Updates an existing equipment record in the database using a strongly-typed model.
        /// </summary>
        /// <param name="equipment">The Equipment object with updated values.</param>
        /// <returns>True if successful, otherwise false.</returns>
        public bool UpdateEquipment(Equipment equipment)
        {
            // Corrected UPDATE statement based on actual 'Equipment' table columns
            string query = @"UPDATE Equipment 
                                SET EquipmentName = @EquipmentName,
                                    CategoryId = @CategoryId,
                                    FactoryId = @FactoryId, 
                                    Model = @Model, 
                                    Manufacturer = @Manufacturer, 
                                    PurchaseDate = @PurchaseDate, 
                                    PurchasePrice = @PurchasePrice, 
                                    Status = @Status, 
                                    Location = @Location, 
                                    ResponsiblePerson = @ResponsiblePerson, 
                                    MaintenanceCycle = @MaintenanceCycle, 
                                    LastMaintenanceDate = @LastMaintenanceDate, 
                                    NextMaintenanceDate = @NextMaintenanceDate, 
                                    Description = @Description
                                    -- Removed: EquipmentType, EquipmentSubType, EqpGroupId, EquipmentLayer, EventUser, EventRemark, EditTime (no EditTime in Equipment table)
                                WHERE EquipmentId = @EquipmentId;";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);
                command.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                command.Parameters.AddWithValue("@CategoryId", equipment.CategoryId);
                command.Parameters.AddWithValue("@FactoryId", (object)equipment.FactoryId ?? DBNull.Value);
                command.Parameters.AddWithValue("@Model", (object)equipment.Model ?? DBNull.Value);
                command.Parameters.AddWithValue("@Manufacturer", (object)equipment.Manufacturer ?? DBNull.Value);
                command.Parameters.AddWithValue("@PurchaseDate", (object)equipment.PurchaseDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@PurchasePrice", (object)equipment.PurchasePrice ?? DBNull.Value);
                command.Parameters.AddWithValue("@Status", equipment.Status ?? "正常");
                command.Parameters.AddWithValue("@Location", (object)equipment.Location ?? DBNull.Value);
                command.Parameters.AddWithValue("@ResponsiblePerson", (object)equipment.ResponsiblePerson ?? DBNull.Value);
                command.Parameters.AddWithValue("@MaintenanceCycle", (object)equipment.MaintenanceCycle ?? DBNull.Value);
                command.Parameters.AddWithValue("@LastMaintenanceDate", (object)equipment.LastMaintenanceDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@NextMaintenanceDate", (object)equipment.NextMaintenanceDate ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value); // 修正：使用Description属性
                
                // Parameters for columns not in Equipment table (or not updatable like CreateTime) are removed:
                // @EquipmentType, @EquipmentSubType, @EqpGroupId, @EquipmentLayer, @EventUser, @EventRemark, @EditTime

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error updating equipment data: " + ex.Message);
                    return false; // Consider re-throwing or custom exception
                }
            }
        }
        
        /// <summary>
        /// Deletes an equipment record from the database. 
        /// </summary>
        /// <param name="equipmentId">The ID of the equipment to delete.</param>
        /// <param name="userName">User performing the delete (for logging/audit).</param>
        /// <returns>True if successful, otherwise false.</returns>
        public bool DeleteEquipment(string equipmentId, string userName /*, DateTime deleteTime */)
        {
            // Consider adding checks for related records (like Maintenance) before deleting
            // if foreign key constraints don't automatically prevent it or if soft delete is needed.
            string query = "DELETE FROM Equipment WHERE EquipmentId = @EquipmentId;";
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@EquipmentId", equipmentId);
                
                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    // Log the delete operation here if needed (using userName)
                    // Example: Log.Info($"User '{userName}' deleted equipment {equipmentId}.");
                    return rowsAffected > 0;
                }
                catch (SqlException ex) when (ex.Number == 547) // FK violation
                {
                    Console.WriteLine($"Error deleting equipment {equipmentId}: Referenced by other data. {ex.Message}");
                    return false;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error deleting equipment {equipmentId}: {ex.Message}");
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Maps a DataRow to an Equipment object.
        /// IMPORTANT: Ensure row column names match the SQL query aliases/names exactly.
        /// </summary>
        private Equipment MapDataRowToEquipment(DataRow row)
        {
            if (row == null) return null;
            
            Equipment equipment = new Equipment();
            
            // 基本字段 - 直接对应数据库列
            equipment.EquipmentId = row["EquipmentId"]?.ToString();
            equipment.EquipmentName = row["EquipmentName"]?.ToString();
            equipment.Description = row.IsNull("Description") ? null : row["Description"].ToString();
            
            // 安全获取可能不存在的列
            if (row.Table.Columns.Contains("CategoryId"))
                equipment.CategoryId = row.IsNull("CategoryId") ? null : row["CategoryId"].ToString();
                
            if (row.Table.Columns.Contains("FactoryId"))
                equipment.FactoryId = row.IsNull("FactoryId") ? null : row["FactoryId"].ToString();
                
            if (row.Table.Columns.Contains("Model"))
                equipment.Model = row.IsNull("Model") ? null : row["Model"].ToString();
                
            if (row.Table.Columns.Contains("Manufacturer"))
                equipment.Manufacturer = row.IsNull("Manufacturer") ? null : row["Manufacturer"].ToString();
            
            // 日期和数字字段
            if (row.Table.Columns.Contains("PurchaseDate"))
                equipment.PurchaseDate = row.IsNull("PurchaseDate") ? null : (DateTime?)row["PurchaseDate"];
                
            if (row.Table.Columns.Contains("PurchasePrice"))
                equipment.PurchasePrice = row.IsNull("PurchasePrice") ? null : (decimal?)row["PurchasePrice"];
                
            if (row.Table.Columns.Contains("MaintenanceCycle"))
                equipment.MaintenanceCycle = row.IsNull("MaintenanceCycle") ? null : (int?)row["MaintenanceCycle"];
                
            if (row.Table.Columns.Contains("LastMaintenanceDate"))
                equipment.LastMaintenanceDate = row.IsNull("LastMaintenanceDate") ? null : (DateTime?)row["LastMaintenanceDate"];
                
            if (row.Table.Columns.Contains("NextMaintenanceDate"))
                equipment.NextMaintenanceDate = row.IsNull("NextMaintenanceDate") ? null : (DateTime?)row["NextMaintenanceDate"];
            
            // 其他字段
            if (row.Table.Columns.Contains("Status"))
                equipment.Status = row.IsNull("Status") ? "正常" : row["Status"].ToString();
                
            if (row.Table.Columns.Contains("Location"))
                equipment.Location = row.IsNull("Location") ? null : row["Location"].ToString();
                
            if (row.Table.Columns.Contains("ResponsiblePerson"))
                equipment.ResponsiblePerson = row.IsNull("ResponsiblePerson") ? null : row["ResponsiblePerson"].ToString();
                
            if (row.Table.Columns.Contains("CreateTime"))
                equipment.CreateTime = row.IsNull("CreateTime") ? DateTime.MinValue : Convert.ToDateTime(row["CreateTime"]);
            
            // 非数据库字段或派生/JOIN字段
            if (row.Table.Columns.Contains("CategoryName"))
                equipment.CategoryName = row.IsNull("CategoryName") ? null : row["CategoryName"].ToString();
                
            if (row.Table.Columns.Contains("FactoryName"))
                equipment.FactoryName = row.IsNull("FactoryName") ? null : row["FactoryName"].ToString();
                
            if (row.Table.Columns.Contains("ResponsiblePersonName"))
                equipment.ResponsiblePersonName = row.IsNull("ResponsiblePersonName") ? null : row["ResponsiblePersonName"].ToString();
                
            // UI/临时记录字段
            if (row.Table.Columns.Contains("EventUser"))
                equipment.EventUser = row.IsNull("EventUser") ? null : row["EventUser"].ToString();
            
            return equipment;
        }

        // --- Existing methods below this line likely need review/update if they use Equipment model ---

        // Example: Keep GetEquipmentCategoriesForComboBox if still used elsewhere
        public List<ComboboxItem> GetEquipmentCategoriesForComboBox()
        {
             List<ComboboxItem> categoryList = new List<ComboboxItem>();
             // Assume EquipmentCategory table exists with CategoryId, CategoryName
             string query = "SELECT CategoryId, CategoryName FROM EquipmentCategory ORDER BY CategoryName;";
             // Corrected: Get connection string and create SqlConnection
             string connectionString = DbConnectionHelper.GetConnectionString();
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 try
                 {
                     connection.Open();
                     using (SqlDataReader reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             categoryList.Add(new ComboboxItem(
                                 reader["CategoryName"].ToString(),
                                 reader["CategoryId"]
                             ));
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     Console.WriteLine("Error fetching equipment categories: " + ex.Message);
                 }
             }
             return categoryList;
        }

        // Example: Keep GetEquipmentByFactory if still used
        public DataTable GetEquipmentByFactory(string factoryId)
        {
             DataTable dt = new DataTable();
             // Corrected to use 'Description'
             string query = @"SELECT e.EquipmentId, e.Description -- Add other needed columns from Equipment table
                              FROM Equipment e 
                              WHERE e.FactoryId = @FactoryId ORDER BY e.EquipmentId;";
             // Corrected: Get connection string and create SqlConnection
             string connectionString = DbConnectionHelper.GetConnectionString();
             using (SqlConnection connection = new SqlConnection(connectionString))
             using (SqlCommand command = new SqlCommand(query, connection))
             {
                 command.Parameters.AddWithValue("@FactoryId", factoryId);
                 using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                 {
                     try
                     {
                         adapter.Fill(dt);
                     }
                     catch (Exception ex)
                     {
                         Console.WriteLine("Error fetching equipment by factory: " + ex.Message);
                     }
                 }
             }
             return dt;
        }

        private Equipment MapReaderToEquipment(SqlDataReader reader)
        {
            Equipment equipment = new Equipment();
            
            // 基本字段 - 直接对应数据库列
            equipment.EquipmentId = reader["EquipmentId"] as string;
            equipment.EquipmentName = reader["EquipmentName"] as string;
            equipment.Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? null : reader["Description"].ToString();
            equipment.CategoryId = reader.IsDBNull(reader.GetOrdinal("CategoryId")) ? null : reader["CategoryId"].ToString();
            equipment.FactoryId = reader.IsDBNull(reader.GetOrdinal("FactoryId")) ? null : reader["FactoryId"].ToString();
            equipment.Model = reader.IsDBNull(reader.GetOrdinal("Model")) ? null : reader["Model"].ToString();
            equipment.Manufacturer = reader.IsDBNull(reader.GetOrdinal("Manufacturer")) ? null : reader["Manufacturer"].ToString();
            
            // 日期和数字字段
            equipment.PurchaseDate = reader.IsDBNull(reader.GetOrdinal("PurchaseDate")) ? null : (DateTime?)reader["PurchaseDate"];
            equipment.PurchasePrice = reader.IsDBNull(reader.GetOrdinal("PurchasePrice")) ? null : (decimal?)reader["PurchasePrice"];
            equipment.MaintenanceCycle = reader.IsDBNull(reader.GetOrdinal("MaintenanceCycle")) ? null : (int?)reader["MaintenanceCycle"];
            equipment.LastMaintenanceDate = reader.IsDBNull(reader.GetOrdinal("LastMaintenanceDate")) ? null : (DateTime?)reader["LastMaintenanceDate"];
            equipment.NextMaintenanceDate = reader.IsDBNull(reader.GetOrdinal("NextMaintenanceDate")) ? null : (DateTime?)reader["NextMaintenanceDate"];
            
            // 其他字段
            equipment.Status = reader.IsDBNull(reader.GetOrdinal("Status")) ? "正常" : reader["Status"].ToString();
            equipment.Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader["Location"].ToString();
            equipment.ResponsiblePerson = reader.IsDBNull(reader.GetOrdinal("ResponsiblePerson")) ? null : reader["ResponsiblePerson"].ToString();
            equipment.CreateTime = reader.IsDBNull(reader.GetOrdinal("CreateTime")) ? DateTime.MinValue : Convert.ToDateTime(reader["CreateTime"]);
            
            // 非数据库字段或派生/JOIN字段
            // 这些可能需要在服务层通过额外查询填充，或通过SQL JOIN获取
            if (reader.HasColumn("CategoryName"))
                equipment.CategoryName = reader.IsDBNull(reader.GetOrdinal("CategoryName")) ? null : reader["CategoryName"].ToString();
                
            if (reader.HasColumn("FactoryName"))
                equipment.FactoryName = reader.IsDBNull(reader.GetOrdinal("FactoryName")) ? null : reader["FactoryName"].ToString();
                
            if (reader.HasColumn("ResponsiblePersonName"))
                equipment.ResponsiblePersonName = reader.IsDBNull(reader.GetOrdinal("ResponsiblePersonName")) ? null : reader["ResponsiblePersonName"].ToString();

            // EventUser可能是操作记录而非数据库字段
            if (reader.HasColumn("EventUser"))
                equipment.EventUser = reader.IsDBNull(reader.GetOrdinal("EventUser")) ? null : reader["EventUser"].ToString();
                
            return equipment;
        }

        // AddEquipment method - ensure FactoryId is included if it's a direct column on Equipment table
        // Corrected to map EquipmentDescription to DB Description and remove EditTime
        public bool AddEquipment(Equipment equipment, User currentUser) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Corrected query: uses DB column 'Description', adds 'CreateTime', removes 'EditTime'
                // Removed EquipmentType, EquipmentSubType as they are not in the main Equipment table definition.
                // If they are truly part of the 'Equipment' table, dbo.sql needs an update.
                string query = @"INSERT INTO Equipment 
                                     (EquipmentId, EquipmentName, Description, CategoryId, FactoryId, Model, Manufacturer, PurchaseDate, PurchasePrice, Status, Location, ResponsiblePerson, MaintenanceCycle, LastMaintenanceDate, NextMaintenanceDate, EventUser, CreateTime) 
                                 VALUES 
                                     (@EquipmentId, @EquipmentName, @Description, @CategoryId, @FactoryId, @Model, @Manufacturer, @PurchaseDate, @PurchasePrice, @Status, @Location, @ResponsiblePerson, @MaintenanceCycle, @LastMaintenanceDate, @NextMaintenanceDate, @EventUser, @CreateTime);";
                
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EquipmentId", string.IsNullOrEmpty(equipment.EquipmentId) ? (object)DBNull.Value : equipment.EquipmentId); 
                    command.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                    command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value); // Model's Description to DB's Description
                    command.Parameters.AddWithValue("@CategoryId", (object)equipment.CategoryId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FactoryId", (object)equipment.FactoryId ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@Model", (object)equipment.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Manufacturer", (object)equipment.Manufacturer ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchaseDate", (object)equipment.PurchaseDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchasePrice", (object)equipment.PurchasePrice ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (object)equipment.Status ?? "正常");
                    command.Parameters.AddWithValue("@Location", (object)equipment.Location ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponsiblePerson", (object)equipment.ResponsiblePerson ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaintenanceCycle", (object)equipment.MaintenanceCycle ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastMaintenanceDate", (object)equipment.LastMaintenanceDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NextMaintenanceDate", (object)equipment.NextMaintenanceDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EventUser", currentUser.Username);
                    command.Parameters.AddWithValue("@CreateTime", equipment.CreateTime); 

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // UpdateEquipment method - ensure FactoryId is included
        // Corrected to map EquipmentDescription to DB Description and remove EditTime
        public bool UpdateEquipment(Equipment equipment, User currentUser)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Corrected query: uses DB column 'Description', removes 'EditTime'
                // CreateTime is generally not updated.
                // Removed EquipmentType, EquipmentSubType as they are not in the main Equipment table definition.
                string query = @"UPDATE Equipment SET 
                                     EquipmentName = @EquipmentName, 
                                     Description = @Description, 
                                     CategoryId = @CategoryId,
                                     FactoryId = @FactoryId, 
                                     Model = @Model,
                                     Manufacturer = @Manufacturer,
                                     PurchaseDate = @PurchaseDate,
                                     PurchasePrice = @PurchasePrice,
                                     Status = @Status,
                                     Location = @Location,
                                     ResponsiblePerson = @ResponsiblePerson,
                                     MaintenanceCycle = @MaintenanceCycle,
                                     LastMaintenanceDate = @LastMaintenanceDate,
                                     NextMaintenanceDate = @NextMaintenanceDate,
                                     EventUser = @EventUser 
                                 WHERE EquipmentId = @EquipmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EquipmentName", equipment.EquipmentName);
                    command.Parameters.AddWithValue("@Description", (object)equipment.Description ?? DBNull.Value); // Model's Description to DB's Description
                    command.Parameters.AddWithValue("@CategoryId", (object)equipment.CategoryId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FactoryId", (object)equipment.FactoryId ?? DBNull.Value); 
                    command.Parameters.AddWithValue("@Model", (object)equipment.Model ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Manufacturer", (object)equipment.Manufacturer ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchaseDate", (object)equipment.PurchaseDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PurchasePrice", (object)equipment.PurchasePrice ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Status", (object)equipment.Status ?? "正常");
                    command.Parameters.AddWithValue("@Location", (object)equipment.Location ?? DBNull.Value);
                    command.Parameters.AddWithValue("@ResponsiblePerson", (object)equipment.ResponsiblePerson ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MaintenanceCycle", (object)equipment.MaintenanceCycle ?? DBNull.Value);
                    command.Parameters.AddWithValue("@LastMaintenanceDate", (object)equipment.LastMaintenanceDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@NextMaintenanceDate", (object)equipment.NextMaintenanceDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@EventUser", currentUser.Username);
                    command.Parameters.AddWithValue("@EquipmentId", equipment.EquipmentId);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        public bool DeleteEquipment(string equipmentId, User currentUser) 
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Consider logging who deleted it, or moving that to a history table trigger
                string query = "DELETE FROM Equipment WHERE EquipmentId = @EquipmentId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@EquipmentId", equipmentId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
} 