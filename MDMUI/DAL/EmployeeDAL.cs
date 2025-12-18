using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model; // Assuming ComboboxItem is in this namespace
using MDMUI.Utility;
// Potentially add using for database helper if one exists, otherwise use direct ADO.NET

namespace MDMUI.DAL
{
    public class EmployeeDAL
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        /// <summary>
        /// Gets a list of employees suitable for a ComboBox (Id and Name).
        /// Only active employees ('在职') should be listed.
        /// </summary>
        /// <returns>A list of ComboboxItem objects representing active employees.</returns>
        public List<ComboboxItem> GetAllEmployeesForComboBox()
        {
            List<ComboboxItem> employeeList = new List<ComboboxItem>();
            // Use direct ADO.NET
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Query to select EmployeeId and EmployeeName for active employees
                string query = "SELECT EmployeeId, EmployeeName FROM Employee WHERE Status = N'在职' ORDER BY EmployeeName;"; 
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        employeeList.Add(new ComboboxItem(
                            // Text comes first
                            reader["EmployeeName"].ToString(),
                            // Value comes second
                            reader["EmployeeId"]
                            // Assuming EmployeeId's type is suitable for 'object'
                        ));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    // Basic error handling - log the exception
                    Console.WriteLine("Error fetching employee data: " + ex.Message); 
                }
            }
            return employeeList;
        }

        // Add other CRUD methods for Employee table as needed
    }
} 
