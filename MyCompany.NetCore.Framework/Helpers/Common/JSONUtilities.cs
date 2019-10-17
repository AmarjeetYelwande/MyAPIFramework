using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;

namespace MyCompany.NetCore.Framework.Helpers.Common
{
    public static class JsonUtilities
    {
        public static string GetJsonValue(string jsonString, string propertyKey, string subValue)
        {
            string propertyValue = "";
            try
            {
                if (subValue == "")
                {
                    if (!jsonString.StartsWith("["))
                    {
                        JObject jObject = JObject.Parse(jsonString);
                        propertyValue = (string)jObject.SelectToken(propertyKey);
                    }
                    if (jsonString.StartsWith("["))
                    {
                        var array = JArray.Parse(jsonString.ToString());
                        propertyValue = (string)array[0].SelectToken(propertyKey);
                    }
                }
                else
                {
                    var jObject = JObject.Parse(jsonString);
                    var value = jObject[subValue][0];
                    propertyValue =  value[propertyKey].ToString();
                }
            }
            catch (Exception propertyReadError)
            {
                Console.WriteLine("Could not read the Property: " + propertyKey + " due to error: " + propertyReadError.Message);
                throw;
            }
            return propertyValue;
        }

        public static JArray GetJsonArrayFromString(string jsonString)
        {
            JArray jsonArray = null;
            try
            {
                jsonArray = JArray.Parse(jsonString) as JArray;
            }
            catch (Exception arrayProcessingError)
            {
                Console.WriteLine($"Conversion of string to array failed due to error {arrayProcessingError.Message}");
                throw;
            }
            return jsonArray;
        }       
       
        public static bool ValidateJsonContentAgainstSchema(string responseContent)
        {
            try
            {
                responseContent = responseContent.Trim();
                if (!responseContent.StartsWith("[")){responseContent = "[" + responseContent + "]";}
                string currentDirectory = Directory.GetCurrentDirectory();
                string pathToJson = Path.Combine(currentDirectory, "Schemas", "StandardJsonSchema.json");
                using (var rawJsonSchema = new StreamReader(pathToJson))
                {
                    var intermediateSchema = rawJsonSchema.ReadToEnd();
                    JSchema finalSchema = JSchema.Parse(intermediateSchema);
                    var responseData = JsonConvert.DeserializeObject<JArray>(responseContent).ToObject<List<JObject>>().FirstOrDefault();
                    return responseData.IsValid(finalSchema);
                }
            }
            catch(Exception processingError)
            {
                Console.WriteLine($"Unable to process response content due to error {processingError.Message}");
                throw;
            }
        }
    }
}
