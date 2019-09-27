using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MyCompany.NetCore.Tests.Common
{
    public static class EndPoint
    {
        public static string GetEndpoint(string application)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string pathtoendpoints = Path.Combine(currentDirectory, "Data", "Endpoints");

                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(pathtoendpoints)
                    .AddJsonFile("Endpoints.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                IConfigurationSection configurationSection = configuration.GetSection(application).GetSection("EndPoint");
                Console.WriteLine($"Endpoint set to : {configurationSection.Value}");
                return configurationSection.Value;
            }
            catch (Exception endpointnotfound)
            {
                Console.WriteLine($"Endpoint not found due to error : {endpointnotfound.Message}");
                return "";
            }
        }
    }
}
