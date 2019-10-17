using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace MyCompany.NetCore.Framework.Helpers.Encryption
{
    public class Md5Hash
    {
        public static string GenerateMd5SecurityHash(string quoteIdentifier, string postCode, DateTime DoB)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                var hashSeed = Sanitize(quoteIdentifier) + Sanitize(postCode) + Sanitize(DoB.ToString("MM-dd-yyyy"));
                string hash = GetMd5Hash(md5Hash, hashSeed);
                return hash;
            }
        }
        private static string Sanitize(string item)
        {
            var regex = new Regex(@"\s");
            var str = item ?? string.Empty;
            str = regex.Replace(str, "");
            str = str.ToLower();
            return str;
        }
        private static string GetMd5Hash(MD5 md5Hash, string input)
        {
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var stringText in data)
            {
                sBuilder.Append(stringText.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
