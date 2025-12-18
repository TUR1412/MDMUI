using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    public class EquipmentMaintenanceDAL
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        /// <summary>
        /// Gets maintenance history for a specific equipment ID.
        /// Returns a DataTable for easy binding to DataGridView.
        /// </summary>
        public DataTable GetHistoryByEquipmentId(string equipmentId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Select relevant columns, join with Employee to get name
                string query = @"SELECT 
                                    em.MaintenanceId, 
                                    em.MaintenanceDate, 
                                    em.MaintenanceType, 
                                    e.EmployeeName AS MaintenancePersonName,  -- Get Employee Name
                                    em.Description, 
                                    em.Cost, 
                                    em.Result, 
                                    em.CreateTime
                                FROM 
                                    EquipmentMaintenance em
                                LEFT JOIN 
                                    Employee e ON em.MaintenancePerson = e.EmployeeId -- Join based on EmployeeId
                                WHERE 
                                    em.EquipmentId = @EquipmentId 
                                ORDER BY 
                                    em.MaintenanceDate DESC, em.CreateTime DESC;";
                
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentId", equipmentId);

                try
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dt);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching maintenance history: " + ex.Message);
                    // Consider throwing exception or returning empty DataTable
                }
            }
            return dt;
        }

        /// <summary>
        /// Inserts a new maintenance record into the database.
        /// </summary>
        /// <returns>True if insertion was successful, otherwise false.</returns>
        public bool InsertMaintenanceRecord(EquipmentMaintenance record)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Note: MaintenanceId is IDENTITY, CreateTime has DEFAULT GETDATE()
                string query = @"INSERT INTO EquipmentMaintenance 
                                    (EquipmentId, MaintenanceDate, MaintenanceType, MaintenancePerson, Description, Cost, Result) 
                                VALUES 
                                    (@EquipmentId, @MaintenanceDate, @MaintenanceType, @MaintenancePerson, @Description, @Cost, @Result);";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EquipmentId", record.EquipmentId);
                command.Parameters.AddWithValue("@MaintenanceDate", record.MaintenanceDate);
                command.Parameters.AddWithValue("@MaintenanceType", record.MaintenanceType);
                command.Parameters.AddWithValue("@MaintenancePerson", record.MaintenancePerson); // Storing EmployeeId
                command.Parameters.AddWithValue("@Description", (object)record.Description ?? DBNull.Value); // Handle potential null
                command.Parameters.AddWithValue("@Cost", (object)record.Cost ?? DBNull.Value); // Handle potential null
                command.Parameters.AddWithValue("@Result", (object)record.Result ?? DBNull.Value); // Handle potential null

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error inserting maintenance record: " + ex.Message);
                    // Consider logging the error and returning false
                    return false;
                }
            }
        }

        // Potentially add Update and Delete methods later if needed
    }
} 
