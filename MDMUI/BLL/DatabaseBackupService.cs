using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using MDMUI.Model;
using MDMUI.Utility;

namespace MDMUI.BLL
{
    public class DatabaseBackupService
    {
        private readonly string connectionString = DbConnectionHelper.GetConnectionString();

        public BackupResult CreateBackup(string targetDirectory, int retentionDays)
        {
            DateTime started = DateTime.Now;
            try
            {
                string dbName = GetDatabaseName();
                if (string.IsNullOrWhiteSpace(dbName))
                {
                    return new BackupResult { Success = false, Message = "未能解析数据库名称" };
                }

                if (string.IsNullOrWhiteSpace(targetDirectory))
                {
                    targetDirectory = GetDefaultBackupDirectory();
                }

                Directory.CreateDirectory(targetDirectory);
                string fileName = $"{dbName}_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
                string fullPath = Path.Combine(targetDirectory, fileName);

                bool backupOk = ExecuteBackup(dbName, fullPath, enableCompression: true, out string backupMessage);
                if (!backupOk)
                {
                    backupOk = ExecuteBackup(dbName, fullPath, enableCompression: false, out backupMessage);
                }

                if (!backupOk)
                {
                    return new BackupResult
                    {
                        Success = false,
                        FilePath = fullPath,
                        Message = backupMessage,
                        Duration = DateTime.Now - started
                    };
                }

                CleanupOldBackups(targetDirectory, dbName, retentionDays);

                return new BackupResult
                {
                    Success = true,
                    FilePath = fullPath,
                    Message = "备份完成",
                    Duration = DateTime.Now - started
                };
            }
            catch (Exception ex)
            {
                return new BackupResult
                {
                    Success = false,
                    Message = "备份失败: " + ex.Message,
                    Duration = DateTime.Now - started
                };
            }
        }

        public List<FileInfo> GetBackupFiles(string targetDirectory)
        {
            List<FileInfo> files = new List<FileInfo>();
            if (string.IsNullOrWhiteSpace(targetDirectory)) return files;
            if (!Directory.Exists(targetDirectory)) return files;

            foreach (string file in Directory.GetFiles(targetDirectory, "*.bak"))
            {
                try
                {
                    files.Add(new FileInfo(file));
                }
                catch
                {
                    // ignore
                }
            }

            files.Sort((a, b) => b.LastWriteTime.CompareTo(a.LastWriteTime));
            return files;
        }

        public string GetDefaultBackupDirectory()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(basePath, "MDMUI", "Backups");
        }

        private bool ExecuteBackup(string dbName, string backupPath, bool enableCompression, out string message)
        {
            string compressionClause = enableCompression ? ", COMPRESSION" : string.Empty;
            string sql = $"BACKUP DATABASE [{dbName}] TO DISK = @Path WITH INIT{compressionClause};";

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString)
                {
                    InitialCatalog = "master"
                };

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Path", backupPath);
                    connection.Open();
                    command.CommandTimeout = 600;
                    command.ExecuteNonQuery();
                }

                message = "备份成功";
                return true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        private void CleanupOldBackups(string directory, string dbName, int retentionDays)
        {
            if (retentionDays <= 0) return;

            DateTime cutoff = DateTime.Now.AddDays(-retentionDays);
            foreach (string file in Directory.GetFiles(directory, $"{dbName}_*.bak"))
            {
                try
                {
                    FileInfo info = new FileInfo(file);
                    if (info.LastWriteTime < cutoff)
                    {
                        info.Delete();
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        private string GetDatabaseName()
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            return builder.InitialCatalog?.Trim();
        }
    }
}
