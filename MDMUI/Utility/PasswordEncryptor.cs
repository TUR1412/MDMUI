using System;
using System.Security.Cryptography;
using System.Text;

namespace MDMUI.Utility
{
    public static class PasswordEncryptor
    {
        // 使用SHA-256加密密码
        public static string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            using (SHA256 sha256 = SHA256.Create())
            {
                // 将密码转换为字节数组
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                // 计算哈希值
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                // 将哈希值转换为字符串
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        // 验证密码
        public static bool VerifyPassword(string inputPassword, string storedHash)
        {
            // 对输入的密码进行哈希处理
            string inputHash = EncryptPassword(inputPassword);

            // 比较哈希值
            return string.Equals(inputHash, storedHash, StringComparison.OrdinalIgnoreCase);
        }
    }
}