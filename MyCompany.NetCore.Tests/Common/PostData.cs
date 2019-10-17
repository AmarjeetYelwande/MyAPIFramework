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
                var rawJsonSchema = new StreamReader(pathToPayloadFile);
                var payload = rawJsonSchema.ReadToEnd();
                return payload;
            }
            catch (Exception postDataNotFound)
            {
                Console.WriteLine($"Unable to load POST data due to error not found due to error : {postDataNotFound.Message}");
                return "";
            }
        }
    }
}
