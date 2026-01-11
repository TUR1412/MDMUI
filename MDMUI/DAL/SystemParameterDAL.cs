using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    public class SystemParameterDAL
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        public List<SystemParameter> GetAll()
        {
            List<SystemParameter> results = new List<SystemParameter>();
            const string sql = @"SELECT ParamKey, ParamValue, Description, UpdatedAt FROM SystemParameters ORDER BY ParamKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new SystemParameter
                        {
                            ParamKey = reader["ParamKey"].ToString(),
                            ParamValue = reader["ParamValue"] == DBNull.Value ? null : reader["ParamValue"].ToString(),
                            Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                            UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedAt"])
                        });
                    }
                }
            }

            return results;
        }

        public SystemParameter GetByKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return null;

            const string sql = @"SELECT ParamKey, ParamValue, Description, UpdatedAt FROM SystemParameters WHERE ParamKey = @ParamKey";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ParamKey", key);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new SystemParameter
                    {
                        ParamKey = reader["ParamKey"].ToString(),
                        ParamValue = reader["ParamValue"] == DBNull.Value ? null : reader["ParamValue"].ToString(),
                        Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString(),
                        UpdatedAt = reader["UpdatedAt"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["UpdatedAt"])
                    };
                }
            }
        }

        public void Upsert(string key, string value, string description)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.SystemParameters WHERE ParamKey = @ParamKey)
BEGIN
    UPDATE dbo.SystemParameters
    SET ParamValue = @ParamValue,
        Description = COALESCE(@Description, Description),
        UpdatedAt = GETDATE()
    WHERE ParamKey = @ParamKey;
END
ELSE
BEGIN
    INSERT INTO dbo.SystemParameters (ParamKey, ParamValue, Description, UpdatedAt)
    VALUES (@ParamKey, @ParamValue, @Description, GETDATE());
END";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@ParamKey", key);
                command.Parameters.AddWithValue("@ParamValue", (object)value ?? DBNull.Value);
                command.Parameters.AddWithValue("@Description", (object)description ?? DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
