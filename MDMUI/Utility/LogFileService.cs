using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDMUI.Utility
{
    public static class LogFileService
    {
        public static string GetEffectiveLogDirectory()
        {
            try { AppLog.Initialize(); } catch { }

            try
            {
                string configured = AppLog.LogDirectory;
                if (!string.IsNullOrWhiteSpace(configured))
                {
                    return configured;
                }
            }
            catch
            {
                // ignore
            }

            string baseDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            return Path.Combine(baseDir, "MDMUI", "logs");
        }

        public static IReadOnlyList<FileInfo> GetLogFiles(string directory = null)
        {
            string dir = directory;
            if (string.IsNullOrWhiteSpace(dir))
            {
                dir = GetEffectiveLogDirectory();
            }

            try
            {
                DirectoryInfo info = new DirectoryInfo(dir);
                if (!info.Exists) return Array.Empty<FileInfo>();

                return info
                    .GetFiles("mdmui-*.log")
                    .OrderByDescending(f => f.LastWriteTimeUtc)
                    .ToList();
            }
            catch
            {
                return Array.Empty<FileInfo>();
            }
        }

        public static IReadOnlyList<string> ReadTailLines(string filePath, int maxLines = 2000, int maxChars = 200_000)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException("filePath is required.", nameof(filePath));
            }

            maxLines = Math.Max(1, maxLines);
            maxChars = Math.Max(1024, maxChars);

            Queue<string> buffer = new Queue<string>(maxLines + 16);
            int charCount = 0;

            using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false), detectEncodingFromByteOrderMarks: true, bufferSize: 4096))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    buffer.Enqueue(line);
                    charCount += line.Length + 1;

                    while (buffer.Count > maxLines)
                    {
                        string removed = buffer.Dequeue();
                        charCount -= removed.Length + 1;
                    }

                    while (charCount > maxChars && buffer.Count > 1)
                    {
                        string removed = buffer.Dequeue();
                        charCount -= removed.Length + 1;
                    }
                }
            }

            return buffer.ToList();
        }

        public static string FormatFileSize(long bytes)
        {
            if (bytes < 0) return "-";
            if (bytes == 0) return "0 B";

            string[] units = { "B", "KB", "MB", "GB" };
            double size = bytes;
            int unit = 0;
            while (size >= 1024 && unit < units.Length - 1)
            {
                size /= 1024;
                unit++;
            }

            string format = unit == 0 ? "0" : "0.##";
            return size.ToString(format) + " " + units[unit];
        }
    }
}

