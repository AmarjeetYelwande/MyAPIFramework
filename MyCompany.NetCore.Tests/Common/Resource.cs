using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace MyCompany.NetCore.Tests.Common
{
    public static class Resource
    {
        public static string GetResource(string resource)
        {
            try
            {
                string currentDirectory = Directory.GetCurrentDirectory();
                string pathtoresources = Path.Combine(currentDirectory, "Data", "Resources");
                
                IConfigurationBuilder builder = new ConfigurationBuilder()
                    .SetBasePath(pathtoresources)
                    .AddJsonFile("Resources.json", optional: true, reloadOnChange: true);

                IConfigurationRoot configuration = builder.Build();
                IConfigurationSection configurationSection = configuration.GetSection(resource).GetSection("Resource");
                Console.WriteLine($"Resource set to : {configurationSection.Value}");
                return configurationSection.Value;
            }
            catch (Exception resourcenotfound)
            {
                Console.WriteLine($"Resource not found due to error : {resourcenotfound.Message}");
                return "";
            }
        }
    }
}
