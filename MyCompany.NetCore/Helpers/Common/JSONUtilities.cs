using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System.Linq;

namespace MyCompany.NetCore.Helpers.Common
{
    public static class JSONUtilities
    {
        public static string GetJsonValue(string JSONString, string propertykey, string subValue)
        {
            string propertyvalue = "";
            try
            {
                if (subValue == "")
                {
                    if (!JSONString.StartsWith("["))
                    {
                        JObject jObject = JObject.Parse(JSONString);
                        propertyvalue = (string)jObject.SelectToken(propertykey);
                    }
                    if (JSONString.StartsWith("["))
                    {
                        JArray array = JArray.Parse(JSONString.ToString());
                        propertyvalue = (string)array[0].SelectToken(propertykey);
                    }
                }
                else
                {
                    JObject jObject = JObject.Parse(JSONString);
                    JToken value = jObject[subValue][0];
                    propertyvalue = value[propertykey].ToString();
                }
            }
            catch (Exception propertyreaderror)
            {
                Console.WriteLine("Could not read the Property: " + propertykey + " due to error: " + propertyreaderror.Message);
                throw;
            }
            return propertyvalue;
        }

        public static JArray GetJsonArrayFromString(string JSONString)
        {
            JArray jsonArray = null;
            try
            {
                jsonArray = JArray.Parse(JSONString) as JArray;
            }
            catch (Exception arrayprocessigerror)
            {
                Console.WriteLine($"Conversion of string to array failed due to error {arrayprocessigerror.Message}");
                throw;
            }
            return jsonArray;
        }       
       
        public static bool ValidateJSONContentAgainstSchema(string responsecontent)
        {
            try
            {
                responsecontent = responsecontent.Trim();
                if (!responsecontent.StartsWith("[")){responsecontent = "[" + responsecontent + "]";}
                string currentDirectory = Directory.GetCurrentDirectory();
                string pathToJson = Path.Combine(currentDirectory, "Schemas", "StandardJsonSchema.json");
                using (var rawjsonschema = new StreamReader(pathToJson))
                {
                    var intermediateschema = rawjsonschema.ReadToEnd();
                    JSchema finalschema = JSchema.Parse(intermediateschema);
                    var responsedata = JsonConvert.DeserializeObject<JArray>(responsecontent).ToObject<List<JObject>>().FirstOrDefault();
                    return responsedata.IsValid(finalschema);
                }
            }
            catch(Exception processingerror)
            {
                Console.WriteLine($"Unable to process response content due to error {processingerror.Message}");
                throw;
            }
        }
    }
}
