using System;
using System.IO;

namespace MyCompany.NetCore.Tests.Common
{
    public static class PostData
    {
        public static string GetPostData(string postdataid)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string pathToPayloadFile = Path.Combine(currentDirectory, "Data\\POST", postdataid + ".json");
                var rawjsonschema = new StreamReader(pathToPayloadFile);
                var payload = rawjsonschema.ReadToEnd();
                return payload;
            }
            catch (Exception postdatanotfound)
            {
                Console.WriteLine($"Unable to load POST data fdue to error not found due to error : {postdatanotfound.Message}");
                return "";
            }
        }
    }
}
