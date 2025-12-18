using System;
using System.Configuration;

namespace MDMUI.Utility
{
    /// <summary>
    /// 提供数据库连接字符串的帮助类
    /// </summary>
    public static class DbConnectionHelper
    {
        private static readonly Lazy<string> CachedConnectionString = new Lazy<string>(ResolveConnectionString);

        /// <summary>
        /// 从 App.config 获取默认连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString()
        {
            return CachedConnectionString.Value;
        }

        private static string ResolveConnectionString()
        {
            // 1) 环境变量优先，便于 CI/部署覆盖（不落盘、不改 config）
            string fromEnv = Environment.GetEnvironmentVariable("MDMUI_CONNECTIONSTRING");
            if (!string.IsNullOrWhiteSpace(fromEnv))
            {
                return fromEnv.Trim();
            }

            // 2) 读取 App.config 中 <connectionStrings> 下名为 "DefaultConnection" 的连接字符串
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DefaultConnection"];
            if (settings != null && !string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                return settings.ConnectionString;
            }

            throw new ConfigurationErrorsException(
                "未找到数据库连接字符串。请在 App.config 的 <connectionStrings> 中配置 name=\"DefaultConnection\"，" +
                "或设置环境变量 MDMUI_CONNECTIONSTRING 进行覆盖。");
        }
    }
} 
