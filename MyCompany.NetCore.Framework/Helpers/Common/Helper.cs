using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCompany.NetCore.Framework.Helpers.Common
{
    public static class Helper
    {
        public static string CreateDirectoryOnDisk(string requiredFolderName)
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string parentFolderName = "YourParentFolderName";
            var path = Path.Combine(root, parentFolderName, requiredFolderName);
            Directory.CreateDirectory(path);
            return path;
        }
        public static string GenerateTransactionId()
        {
            return Guid.NewGuid().ToString();
        }
        public static string DictionaryToQueryParametersList(Dictionary<string, string> parameterList)
        {
            if (parameterList.Count != 0)
            {
                var keys = new List<string>(parameterList.Keys);
                StringBuilder sb = new StringBuilder();
                foreach (string key in keys)
                {
                    var value = parameterList[key];
                    sb.Append(key + "=" + value + "&");
                }
                var intermediateString = sb.ToString();
                var finalString = intermediateString.Remove(intermediateString.Length - 1);
                return finalString;
            }
            return "";
        }

        public static string DictionaryToUriParametersList(Dictionary<string, string> parameterList)
        {
            if (parameterList.Count != 0)
            {
                var keys = new List<string>(parameterList.Keys);
                var sb = new StringBuilder();
                foreach (var key in keys)
                {
                    var value = parameterList[key];
                    sb.Append(value + "/");
                }
                var intermediateString = sb.ToString();
                
                return intermediateString.Remove(intermediateString.Length - 1);
            }
            return "";
        }


    }
}
