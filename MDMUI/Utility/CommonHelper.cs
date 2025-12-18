using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MDMUI.Utility
{
    /// <summary>
    /// 通用帮助类，提供各种常用的工具方法
    /// </summary>
    public static class CommonHelper
    {
        /// <summary>
        /// 生成一个随机的唯一标识符字符串
        /// </summary>
        /// <returns>唯一标识符字符串</returns>
        public static string GenerateUniqueId()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 将异常信息递归组合成完整的错误消息
        /// </summary>
        /// <param name="ex">异常对象</param>
        /// <returns>完整的错误消息</returns>
        public static string GetFullExceptionMessage(Exception ex)
        {
            if (ex == null)
                return string.Empty;

            string message = ex.Message;
            if (ex.InnerException != null)
            {
                message += " -> " + GetFullExceptionMessage(ex.InnerException);
            }
            return message;
        }

        /// <summary>
        /// 显示错误消息对话框
        /// </summary>
        /// <param name="message">错误消息</param>
        /// <param name="caption">标题</param>
        public static void ShowError(string message, string caption = "错误")
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// 显示信息消息对话框
        /// </summary>
        /// <param name="message">信息消息</param>
        /// <param name="caption">标题</param>
        public static void ShowInfo(string message, string caption = "信息")
        {
            MessageBox.Show(message, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// 显示确认对话框
        /// </summary>
        /// <param name="message">确认消息</param>
        /// <param name="caption">标题</param>
        /// <returns>如果用户点击"是"，则返回true；否则返回false</returns>
        public static bool Confirm(string message, string caption = "确认")
        {
            return MessageBox.Show(message, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// 从十六进制字符串获取颜色
        /// </summary>
        /// <param name="hexColor">十六进制颜色字符串 (如 "#FF0000")</param>
        /// <returns>Color对象</returns>
        public static Color GetColorFromHex(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor) || hexColor.Length < 6)
                return Color.Black;

            hexColor = hexColor.Replace("#", "").Replace(" ", "");

            try
            {
                int r = Convert.ToInt32(hexColor.Substring(0, 2), 16);
                int g = Convert.ToInt32(hexColor.Substring(2, 2), 16);
                int b = Convert.ToInt32(hexColor.Substring(4, 2), 16);

                return Color.FromArgb(r, g, b);
            }
            catch
            {
                return Color.Black;
            }
        }

        /// <summary>
        /// 确保目录存在
        /// </summary>
        /// <param name="path">目录路径</param>
        /// <returns>如果目录已存在或成功创建，则返回true；否则返回false</returns>
        public static bool EnsureDirectoryExists(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取应用程序配置目录的完整路径
        /// </summary>
        /// <returns>配置目录的完整路径</returns>
        public static string GetConfigDirectory()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string configDirPath = Path.Combine(appDataPath, "MDMUI", "Config");
            EnsureDirectoryExists(configDirPath);
            return configDirPath;
        }

        /// <summary>
        /// 分页计算总页数
        /// </summary>
        /// <param name="totalItems">总项目数</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns>总页数</returns>
        public static int CalculateTotalPages(int totalItems, int pageSize)
        {
            if (pageSize <= 0)
                return 0;
            
            return (int)Math.Ceiling((double)totalItems / pageSize);
        }
    }
} 