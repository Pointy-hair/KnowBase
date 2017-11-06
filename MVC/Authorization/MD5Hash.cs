using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVC.Authorization
{
    public static class MD5Hash
    {
        public static string GetHash(string input)
        {
            MD5CryptoServiceProvider hashProvider = new MD5CryptoServiceProvider();

            byte[] dataBytes = hashProvider.ComputeHash(Encoding.Default.GetBytes(input));

            StringBuilder stringBuilder = new StringBuilder();

            foreach (var dataByte in dataBytes)
            {
                stringBuilder.Append(dataByte.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static bool Verify(string input, string hash)
        {
            string hashInput = GetHash(input);
            
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashInput, hash) == 0;
        }
    }
}