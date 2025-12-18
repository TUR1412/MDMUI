using System.Configuration;

namespace MDMUI.Utility
{
    /// <summary>
    /// 提供数据库连接字符串的帮助类
    /// </summary>
    public static class DbConnectionHelper
    {
        /// <summary>
        /// 从 App.config 获取默认连接字符串
        /// </summary>
        /// <returns>连接字符串</returns>
        public static string GetConnectionString()
        {
            // 读取 App.config 中 <connectionStrings> 下名为 "DefaultConnection" 的连接字符串
            // 如果您的连接字符串名称不同，请修改 "DefaultConnection"
            ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["DefaultConnection"];

            if (settings != null)
            {
                return settings.ConnectionString;
            }
            else
            {
                // 如果找不到连接字符串，可以抛出异常或返回一个默认值（不推荐）
                throw new ConfigurationErrorsException("在 App.config 中未找到名为 \"DefaultConnection\" 的连接字符串。");
            }
        }
    }
} 