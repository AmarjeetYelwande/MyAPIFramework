using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCompany.NetCore.Framework.Helpers.Encryption
{
    public class MD5Hash
    {
        public static string GenerateMD5SecurityHash(string quoteIdentifier, string postCode, DateTime DoB)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var hashSeed = Sanitise(quoteIdentifier) + Sanitise(postCode) + Sanitise(DoB.ToString("MM-dd-yyyy"));
                string hash = GetMd5Hash(md5Hash, hashSeed);
                return hash;
            }
        }
        private static string Sanitise(string item)
        {
            var regex = new Regex(@"\s");
            var str = item ?? string.Empty;
            str = regex.Replace(str, "");
            str = str.ToLower();
            return str;
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
