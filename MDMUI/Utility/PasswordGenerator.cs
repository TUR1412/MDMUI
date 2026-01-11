using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace MDMUI.Utility
{
    public static class PasswordGenerator
    {
        private const string Digits = "0123456789";
        private const string Upper = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lower = "abcdefghijkmnopqrstuvwxyz";
        private const string Special = "!@#$%^&*()-_=+[]{};:,.?";

        public static string GenerateStrong(int minLength = 12)
        {
            int length = Math.Max(12, minLength);
            return Generate(length);
        }

        public static string Generate(int length)
        {
            // 至少需要 4 个字符来确保四类字符都能出现
            int targetLength = Math.Max(4, length);

            List<char> chars = new List<char>(targetLength);
            chars.Add(RandomChar(Digits));
            chars.Add(RandomChar(Upper));
            chars.Add(RandomChar(Lower));
            chars.Add(RandomChar(Special));

            string pool = Digits + Upper + Lower + Special;
            while (chars.Count < targetLength)
            {
                chars.Add(RandomChar(pool));
            }

            Shuffle(chars);
            return new string(chars.ToArray());
        }

        private static char RandomChar(string pool)
        {
            if (string.IsNullOrEmpty(pool)) throw new ArgumentException("pool 不能为空", nameof(pool));
            int index = RandomNumber(pool.Length);
            return pool[index];
        }

        private static int RandomNumber(int maxExclusive)
        {
            if (maxExclusive <= 0) return 0;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[4];
                int value;
                do
                {
                    rng.GetBytes(buffer);
                    value = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
                } while (value >= int.MaxValue - (int.MaxValue % maxExclusive));

                return value % maxExclusive;
            }
        }

        private static void Shuffle(List<char> chars)
        {
            if (chars == null || chars.Count <= 1) return;

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[4];
                for (int i = chars.Count - 1; i > 0; i--)
                {
                    int j = NextInt(rng, buffer, i + 1);
                    (chars[i], chars[j]) = (chars[j], chars[i]);
                }
            }
        }

        private static int NextInt(RandomNumberGenerator rng, byte[] buffer, int maxExclusive)
        {
            if (maxExclusive <= 0) return 0;

            int value;
            do
            {
                rng.GetBytes(buffer);
                value = BitConverter.ToInt32(buffer, 0) & int.MaxValue;
            } while (value >= int.MaxValue - (int.MaxValue % maxExclusive));

            return value % maxExclusive;
        }
    }
}
