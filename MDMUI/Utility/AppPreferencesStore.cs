using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;

namespace MDMUI.Utility
{
    [Serializable]
    public sealed class AppPreferences
    {
        public string LastUsername { get; set; }
        public string LastFactoryName { get; set; }
        public string LastLanguage { get; set; }
        public bool RememberPassword { get; set; }
        public string ProtectedPassword { get; set; }
        public DateTime LastUpdatedUtc { get; set; }
        public List<CommandUsageEntry> CommandUsage { get; set; } = new List<CommandUsageEntry>();
    }

    [Serializable]
    public sealed class CommandUsageEntry
    {
        public string FormName { get; set; }
        public string Title { get; set; }
        public string Group { get; set; }
        public int UseCount { get; set; }
        public DateTime LastUsedUtc { get; set; }
        public bool Pinned { get; set; }
    }

    /// <summary>
    /// 轻量本地偏好设置：用于“记住密码/上次登录信息”等纯客户端体验增强。
    /// - 存储位置：%LOCALAPPDATA%\MDMUI\preferences.xml
    /// - 密码：使用 DPAPI (CurrentUser) 保护，仅当前 Windows 用户可解密
    /// </summary>
    public static class AppPreferencesStore
    {
        private static readonly object Gate = new object();

        private static readonly byte[] EntropyBytes =
            Encoding.UTF8.GetBytes("MDMUI.AppPreferencesStore.v1");

        public static string PreferencesFilePath
        {
            get
            {
                string folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "MDMUI");

                return Path.Combine(folder, "preferences.xml");
            }
        }

        public static AppPreferences Load()
        {
            lock (Gate)
            {
                string filePath = PreferencesFilePath;
                if (!File.Exists(filePath))
                {
                    return new AppPreferences();
                }

                try
                {
                    using (FileStream fs = File.OpenRead(filePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(AppPreferences));
                        var prefs = serializer.Deserialize(fs) as AppPreferences;
                        AppPreferences resolved = prefs ?? new AppPreferences();
                        if (resolved.CommandUsage == null)
                        {
                            resolved.CommandUsage = new List<CommandUsageEntry>();
                        }
                        return resolved;
                    }
                }
                catch
                {
                    // 偏好设置损坏/反序列化失败时，直接回退到默认值（不影响主流程）
                    return new AppPreferences();
                }
            }
        }

        public static void Save(AppPreferences preferences)
        {
            if (preferences == null) return;

            lock (Gate)
            {
                string filePath = PreferencesFilePath;
                string folder = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                preferences.LastUpdatedUtc = DateTime.UtcNow;

                using (FileStream fs = File.Create(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppPreferences));
                    serializer.Serialize(fs, preferences);
                }
            }
        }

        public static void Clear()
        {
            lock (Gate)
            {
                string filePath = PreferencesFilePath;
                if (File.Exists(filePath))
                {
                    try { File.Delete(filePath); } catch { }
                }
            }
        }

        public static string ProtectString(string plainText)
        {
            if (string.IsNullOrEmpty(plainText)) return string.Empty;

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(plainText);
                byte[] protectedData = ProtectedData.Protect(data, EntropyBytes, DataProtectionScope.CurrentUser);
                return Convert.ToBase64String(protectedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string UnprotectString(string protectedBase64)
        {
            if (string.IsNullOrEmpty(protectedBase64)) return string.Empty;

            try
            {
                byte[] protectedData = Convert.FromBase64String(protectedBase64);
                byte[] data = ProtectedData.Unprotect(protectedData, EntropyBytes, DataProtectionScope.CurrentUser);
                return Encoding.UTF8.GetString(data);
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
