using System;
using System.Text;

namespace MDMUI.Utility
{
    /// <summary>
    /// 提供字符串处理的辅助方法
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// 检查字符串是否为空或仅由空白字符组成
        /// </summary>
        /// <param name="str">要检查的字符串</param>
        /// <returns>如果字符串为null或仅由空白字符组成，则返回true</returns>
        public static bool IsNullOrWhiteSpace(string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 确保字符串不为null，如果为null则返回空字符串
        /// </summary>
        /// <param name="str">要处理的字符串</param>
        /// <returns>如果输入不为null，则返回输入字符串；否则返回空字符串</returns>
        public static string EnsureNotNull(string str)
        {
            return str ?? string.Empty;
        }

        /// <summary>
        /// 截断字符串到指定长度
        /// </summary>
        /// <param name="str">要截断的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns>截断后的字符串</returns>
        public static string Truncate(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
                return str;

            return str.Substring(0, maxLength);
        }

        /// <summary>
        /// 将驼峰命名法转换为带空格的标题
        /// </summary>
        /// <param name="camelCaseStr">驼峰命名法的字符串</param>
        /// <returns>带空格的标题</returns>
        public static string CamelCaseToTitle(string camelCaseStr)
        {
            if (string.IsNullOrEmpty(camelCaseStr))
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            sb.Append(char.ToUpper(camelCaseStr[0]));

            for (int i = 1; i < camelCaseStr.Length; i++)
            {
                if (char.IsUpper(camelCaseStr[i]))
                    sb.Append(' ');
                
                sb.Append(camelCaseStr[i]);
            }

            return sb.ToString();
        }
    }
} 