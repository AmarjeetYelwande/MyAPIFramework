using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Globalization;

namespace MyCompany.NetCore.Helpers.Authentication
{
    public partial class JwtParser
    {
        [JsonProperty("ver")]
        public long ver { get; set; }

        [JsonProperty("jti")]
        public string jti { get; set; }

        [JsonProperty("iss")]
        public string iss { get; set; }

        [JsonProperty("aud")]
        public string aud { get; set; }

        [JsonProperty("iat")]
        public long iat { get; set; }

        [JsonProperty("exp")]
        public long exp { get; set; }

        [JsonProperty("cid")]
        public string cid { get; set; }

        [JsonProperty("uid")]
        public string uid { get; set; }

        [JsonProperty("scp")]
        public List<string> scp { get; set; }

        [JsonProperty("sub")]
        public string sub { get; set; }

        [JsonProperty("identifierUid")]
        public string identifierUid { get; set; }

        [JsonProperty("roles")]
        public List<string> roles { get; set; }

        [JsonProperty("brand")]
        public string brand { get; set; }

        [JsonProperty("status")]
        public string status { get; set; }
    }

    public partial class JwtParser
    {
        public static JwtParser FromJson(string json)
        {
            return JsonConvert.DeserializeObject<JwtParser>(json, Converter.Settings);
        }
    }

    public static class Serialize
    {
        public static string ToJson(this JwtParser self)
        {
            return JsonConvert.SerializeObject(self, Converter.Settings);
        }
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
