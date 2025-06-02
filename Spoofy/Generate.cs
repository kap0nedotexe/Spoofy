using System;
using System.Collections.Generic;
using System.Linq;

namespace Spoofy
{
    internal static class Generate
    {
        private const string allCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly HashSet<string> generatedStrings = new HashSet<string>();
        private static readonly object lockObject = new object();

        internal static string RandomComputerName(int length)
        {
            Random random = new Random();

            if (length <= 0)
            {
                length = 8;
            }

            lock (lockObject)
            {
                char[] result = new char[length];
                string resultString;
                int maxAttempts = 100;

                do
                {
                    for (int i = 0; i < length; i++)
                    {
                        result[i] = allCharacters[random.Next(allCharacters.Length)];
                    }

                    resultString = new string(result);
                    maxAttempts--;
                }

                while (generatedStrings.Contains(resultString) && maxAttempts > 0);

                generatedStrings.Add(resultString);

                return "PC-" + resultString;
            }
        }

        internal static string RandomProductID()
        {
            Random random = new Random();

            return $"{random.Next(10000, 99999)}-{random.Next(100, 999)}-{random.Next(1000000, 9999999)}-{random.Next(10000, 99999)}";
        }

        internal static string RandomMACAddress()
        {
            Random random = new Random();
            byte[] mac = new byte[6];

            random.NextBytes(mac);

            mac[0] = (byte)((mac[0] & 0xFE) | 0x02);

            return string.Join("-", mac.Select(b => b.ToString("X2")));
        }
    }
}