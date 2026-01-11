using System;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace MDMUI.Utility
{
    public enum AppLogLevel
    {
        Trace = 0,
        Debug = 1,
        Info = 2,
        Warn = 3,
        Error = 4,
        Critical = 5
    }

    public static class AppLog
    {
        private static readonly object SyncRoot = new object();
        private static int initialized;

        private static string logDirectory;
        private static string currentLogFilePath;
        private static StreamWriter writer;
        private static DateTime writerDate;

        private static int maxFileBytes = 10 * 1024 * 1024;
        private static int retentionDays = 14;
        private static bool enabled = true;

        public static string LogDirectory
        {
            get { lock (SyncRoot) { return logDirectory; } }
        }

        public static string CurrentLogFilePath
        {
            get { lock (SyncRoot) { return currentLogFilePath; } }
        }

        public static void Initialize()
        {
            if (Interlocked.Exchange(ref initialized, 1) == 1) return;

            try
            {
                enabled = !IsTruthy(GetSetting("MDMUI_LOG_DISABLED", "MDMUI.LogDisabled", defaultValue: "0"));
                if (!enabled) return;

                string configuredDir = GetSetting("MDMUI_LOG_DIR", "MDMUI.LogDirectory", defaultValue: null);
                logDirectory = ResolveLogDirectory(configuredDir);

                string maxMbRaw = GetSetting("MDMUI_LOG_MAX_MB", "MDMUI.LogMaxMB", defaultValue: "10");
                if (int.TryParse(maxMbRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int maxMb) && maxMb > 0)
                {
                    maxFileBytes = maxMb * 1024 * 1024;
                }

                string retentionRaw = GetSetting("MDMUI_LOG_RETENTION_DAYS", "MDMUI.LogRetentionDays", defaultValue: "14");
                if (int.TryParse(retentionRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int days) && days > 0)
                {
                    retentionDays = days;
                }

                Directory.CreateDirectory(logDirectory);
                PruneOldLogs(logDirectory, retentionDays);

                System.Diagnostics.Trace.AutoFlush = true;
                System.Diagnostics.Debug.AutoFlush = true;

                // 捕获 Debug/Trace 输出到文件（保留 DefaultTraceListener，不影响 VS 输出窗口）
                TryAttachListener(System.Diagnostics.Debug.Listeners);
                TryAttachListener(System.Diagnostics.Trace.Listeners);

                Info($"AppLog 初始化完成。LogDir={logDirectory}");
            }
            catch
            {
                // 日志初始化失败不应阻断主流程
                enabled = false;
            }
        }

        public static void Trace(string message) => Write(AppLogLevel.Trace, message, null);
        public static void Debug(string message) => Write(AppLogLevel.Debug, message, null);
        public static void Info(string message) => Write(AppLogLevel.Info, message, null);
        public static void Warn(string message) => Write(AppLogLevel.Warn, message, null);
        public static void Error(string message) => Write(AppLogLevel.Error, message, null);
        public static void Error(Exception ex, string message = null) => Write(AppLogLevel.Error, message, ex);
        public static void Critical(Exception ex, string message = null) => Write(AppLogLevel.Critical, message, ex);

        public static void Flush()
        {
            if (!enabled) return;

            lock (SyncRoot)
            {
                try { writer?.Flush(); } catch { }
            }
        }

        public static void Shutdown()
        {
            if (!enabled) return;

            lock (SyncRoot)
            {
                try
                {
                    writer?.Flush();
                    writer?.Dispose();
                }
                catch { }
                finally
                {
                    writer = null;
                }
            }
        }

        internal static void WriteFromTrace(string message)
        {
            // TraceListener Write/WriteLine 调用路径：避免递归使用 Trace/Debug
            Write(AppLogLevel.Debug, message, null, isTracePipeline: true);
        }

        private static void Write(AppLogLevel level, string message, Exception ex, bool isTracePipeline = false)
        {
            if (!enabled) return;

            try
            {
                if (string.IsNullOrWhiteSpace(message) && ex == null) return;

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
                int threadId = Thread.CurrentThread.ManagedThreadId;
                string header = $"{time} [{level.ToString().ToUpperInvariant()}] [T{threadId}] ";

                StringBuilder sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(message))
                {
                    sb.Append(header);
                    sb.Append(message.Trim());
                    sb.AppendLine();
                }

                if (ex != null)
                {
                    sb.Append(header);
                    sb.AppendLine("Exception:");
                    sb.AppendLine(ex.ToString());
                }

                string payload = sb.ToString();

                lock (SyncRoot)
                {
                    EnsureWriter();
                    RotateIfNeeded();

                    writer.Write(payload);
                    writer.Flush();
                }

                // 仅在非 TraceListener 管道中做额外输出，避免递归
                if (!isTracePipeline)
                {
                    try { System.Diagnostics.Debugger.Log(0, "MDMUI", payload); } catch { }
                }
            }
            catch
            {
                // 写日志失败不应影响业务
            }
        }

        private static void EnsureWriter()
        {
            if (string.IsNullOrWhiteSpace(logDirectory))
            {
                logDirectory = ResolveLogDirectory(null);
                Directory.CreateDirectory(logDirectory);
            }

            DateTime today = DateTime.Today;
            if (writer != null && writerDate == today) return;

            try { writer?.Dispose(); } catch { }
            writer = null;

            writerDate = today;
            string fileName = $"mdmui-{today:yyyyMMdd}.log";
            currentLogFilePath = Path.Combine(logDirectory, fileName);

            writer = new StreamWriter(new FileStream(currentLogFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite), new UTF8Encoding(encoderShouldEmitUTF8Identifier: false))
            {
                AutoFlush = false
            };
        }

        private static void RotateIfNeeded()
        {
            if (string.IsNullOrWhiteSpace(currentLogFilePath)) return;

            FileInfo info;
            try { info = new FileInfo(currentLogFilePath); }
            catch { return; }

            if (!info.Exists) return;
            if (info.Length < maxFileBytes) return;

            string rotatedName = $"mdmui-{writerDate:yyyyMMdd}-{DateTime.Now:HHmmss}-{Process.GetCurrentProcess().Id}.log";
            string rotatedPath = Path.Combine(logDirectory, rotatedName);

            try
            {
                writer?.Flush();
                writer?.Dispose();
            }
            catch { }
            finally
            {
                writer = null;
            }

            try
            {
                File.Move(currentLogFilePath, rotatedPath);
            }
            catch
            {
                // 如果移动失败（例如被占用），降级为继续写入原文件
            }

            EnsureWriter();
            PruneOldLogs(logDirectory, retentionDays);
        }

        private static void TryAttachListener(TraceListenerCollection listeners)
        {
            try
            {
                foreach (TraceListener listener in listeners)
                {
                    if (listener is AppLogTraceListener) return;
                }

                listeners.Add(new AppLogTraceListener());
            }
            catch
            {
                // 监听器添加失败也不应影响主流程
            }
        }

        private static string ResolveLogDirectory(string configuredDir)
        {
            if (!string.IsNullOrWhiteSpace(configuredDir))
            {
                return Environment.ExpandEnvironmentVariables(configuredDir);
            }

            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(baseDir, "MDMUI", "logs");
        }

        private static string GetSetting(string envKey, string appConfigKey, string defaultValue)
        {
            try
            {
                string env = Environment.GetEnvironmentVariable(envKey);
                if (!string.IsNullOrWhiteSpace(env)) return env;
            }
            catch { }

            try
            {
                string value = ConfigurationManager.AppSettings[appConfigKey];
                if (!string.IsNullOrWhiteSpace(value)) return value;
            }
            catch { }

            return defaultValue;
        }

        private static bool IsTruthy(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;

            string v = value.Trim();
            return v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase) || v.Equals("yes", StringComparison.OrdinalIgnoreCase);
        }

        private static void PruneOldLogs(string directory, int days)
        {
            if (days <= 0) return;
            if (string.IsNullOrWhiteSpace(directory)) return;

            try
            {
                DateTime cutoff = DateTime.Now.AddDays(-days);
                foreach (FileInfo file in new DirectoryInfo(directory).GetFiles("mdmui-*.log").OrderByDescending(f => f.LastWriteTime))
                {
                    if (file.LastWriteTime >= cutoff) continue;
                    try { file.Delete(); } catch { }
                }
            }
            catch
            {
                // 清理失败不影响主流程
            }
        }
    }

    internal sealed class AppLogTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            AppLog.WriteFromTrace(message);
        }

        public override void WriteLine(string message)
        {
            AppLog.WriteFromTrace(message);
        }
    }
}
