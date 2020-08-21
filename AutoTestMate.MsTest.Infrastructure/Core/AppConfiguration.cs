using System.Collections.Specialized;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class AppConfiguration : IConfiguration
    {
        public AppConfiguration()
        { 
            Settings = new NameValueCollection();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configBuilder.Build();

            foreach (var configurationSection in configuration.GetSection("Settings").GetChildren())
            {
                Settings.Add(configurationSection.Key, configurationSection.Value);
            }

           
        }
        public NameValueCollection Settings { get; set; }
    }
}
