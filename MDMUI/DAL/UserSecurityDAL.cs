using System;
using System.Data;
using System.Data.SqlClient;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.DAL
{
    public class UserSecurityDAL
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        public UserSecurityStatus GetStatus(int userId)
        {
            if (userId <= 0) return null;

            const string sql = @"SELECT UserId, FailedCount, LastFailedAt, LockoutUntil, LastSuccessAt FROM dbo.UserSecurity WHERE UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.Read()) return null;

                    return new UserSecurityStatus
                    {
                        UserId = Convert.ToInt32(reader["UserId"]),
                        FailedCount = reader["FailedCount"] == DBNull.Value ? 0 : Convert.ToInt32(reader["FailedCount"]),
                        LastFailedAt = reader["LastFailedAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastFailedAt"]),
                        LockoutUntil = reader["LockoutUntil"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LockoutUntil"]),
                        LastSuccessAt = reader["LastSuccessAt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["LastSuccessAt"])
                    };
                }
            }
        }

        public void EnsureStatus(int userId)
        {
            if (userId <= 0) return;

            const string sql = @"
IF NOT EXISTS (SELECT 1 FROM dbo.UserSecurity WHERE UserId = @UserId)
BEGIN
    INSERT INTO dbo.UserSecurity (UserId, FailedCount) VALUES (@UserId, 0);
END";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public UserSecurityStatus RecordFailure(int userId, int maxFailed, TimeSpan lockoutDuration)
        {
            EnsureStatus(userId);

            UserSecurityStatus status = GetStatus(userId) ?? new UserSecurityStatus { UserId = userId, FailedCount = 0 };
            int nextCount = status.FailedCount + 1;
            DateTime? lockoutUntil = status.LockoutUntil;

            if (maxFailed > 0 && nextCount >= maxFailed)
            {
                lockoutUntil = DateTime.Now.Add(lockoutDuration);
            }

            const string sql = @"
UPDATE dbo.UserSecurity
SET FailedCount = @FailedCount,
    LastFailedAt = GETDATE(),
    LockoutUntil = @LockoutUntil
WHERE UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@FailedCount", nextCount);
                command.Parameters.AddWithValue("@LockoutUntil", (object)lockoutUntil ?? DBNull.Value);
                connection.Open();
                command.ExecuteNonQuery();
            }

            status.FailedCount = nextCount;
            status.LastFailedAt = DateTime.Now;
            status.LockoutUntil = lockoutUntil;
            return status;
        }

        public UserSecurityStatus RecordSuccess(int userId)
        {
            EnsureStatus(userId);

            const string sql = @"
UPDATE dbo.UserSecurity
SET FailedCount = 0,
    LastFailedAt = NULL,
    LockoutUntil = NULL,
    LastSuccessAt = GETDATE()
WHERE UserId = @UserId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }

            return GetStatus(userId);
        }
    }
}
