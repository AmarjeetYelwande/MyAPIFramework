using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MyCompany.NetCore.Helpers.Common
{
    public static class Helper
    {
        public static string CreateDirectoryOnDisk(string requiredfoldername)
        {
            string root = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string parentfoldername = "YourParentFolderName";
            var path = Path.Combine(root, parentfoldername, requiredfoldername);
            Directory.CreateDirectory(path);
            return path;
        }
        public static string GenerateTransactionId()
        {
            return Guid.NewGuid().ToString();
        }
        public static string DictionaryToQueryParametersList(Dictionary<string, string> parameterlist)
        {
            if (parameterlist.Count != 0)
            {
                var keys = new List<string>(parameterlist.Keys);
                StringBuilder sb = new StringBuilder();
                foreach (string key in keys)
                {
                    var value = parameterlist[key];
                    sb.Append(key + "=" + value + "&");
                }
                var intermediatestring = sb.ToString();
                var finalstring = intermediatestring.Remove(intermediatestring.Length - 1);
                return finalstring;
            }
            return "";
        }

        public static string DictionaryToUriParametersList(Dictionary<string, string> parameterlist)
        {
            if (parameterlist.Count != 0)
            {
                var keys = new List<string>(parameterlist.Keys);
                StringBuilder sb = new StringBuilder();
                foreach (string key in keys)
                {
                    var value = parameterlist[key];
                    sb.Append(value + "/");
                }
                var intermediatestring = sb.ToString();
                var finalstring = intermediatestring.Remove(intermediatestring.Length - 1);
                return finalstring;
            }
            return "";
        }


    }
}
