using System;
using System.Data.SqlClient;

namespace MDMUI.Utility
{
    /// <summary>
    /// SqlDataReader 的安全读取扩展：避免 DBNull / 列缺失导致的异常。
    /// </summary>
    public static class SqlDataReaderExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            if (reader == null) return false;
            if (string.IsNullOrWhiteSpace(columnName)) return false;

            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (string.Equals(reader.GetName(i), columnName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetStringOrDefault(this SqlDataReader reader, string columnName, string defaultValue = "")
        {
            if (reader == null) return defaultValue;
            if (!reader.HasColumn(columnName)) return defaultValue;

            object value = reader[columnName];
            if (value == null || value == DBNull.Value) return defaultValue;
            return Convert.ToString(value) ?? defaultValue;
        }

        public static int GetInt32OrDefault(this SqlDataReader reader, string columnName, int defaultValue = 0)
        {
            if (reader == null) return defaultValue;
            if (!reader.HasColumn(columnName)) return defaultValue;

            object value = reader[columnName];
            if (value == null || value == DBNull.Value) return defaultValue;

            if (value is int i) return i;
            if (int.TryParse(Convert.ToString(value), out int parsed)) return parsed;
            return defaultValue;
        }

        public static DateTime? GetNullableDateTime(this SqlDataReader reader, string columnName)
        {
            if (reader == null) return null;
            if (!reader.HasColumn(columnName)) return null;

            object value = reader[columnName];
            if (value == null || value == DBNull.Value) return null;

            if (value is DateTime dt) return dt;
            if (DateTime.TryParse(Convert.ToString(value), out DateTime parsed)) return parsed;
            return null;
        }
    }
}
