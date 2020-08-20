using System.Collections.Concurrent; 
using System;
namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
 public class ConfigurationService : IConfigurationService, IDisposable
    {
        public ConfigurationService()
        {
            ConfigurationReaders = new ConcurrentDictionary<string, IConfigurationReader>();
        }
        
        public ConcurrentDictionary<string, IConfigurationReader> ConfigurationReaders { get; set; }

        public void AddOrUpdate(string testMethod, IConfigurationReader configurationReader)
        {
            ConfigurationReaders.AddOrUpdate(testMethod, configurationReader, (key, oldValue) => configurationReader);
        }
        public bool TryGetValue(string testMethod, out IConfigurationReader configurationReader)
        {
            return ConfigurationReaders.TryGetValue(testMethod, out configurationReader);
        }
        public void Dispose(string testMethod)
        {
            ConfigurationReaders.TryRemove(testMethod, out _);
        }

        public void Dispose()
        {
            ConfigurationReaders.Clear();
        }
    }
}