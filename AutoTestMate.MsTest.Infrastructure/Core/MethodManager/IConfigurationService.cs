using System.Collections.Concurrent;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public interface IConfigurationService
    {
        ConcurrentDictionary<string, IConfigurationReader> ConfigurationReaders { get; set; }
        void AddOrUpdate(string testMethod, IConfigurationReader configurationReader);
        bool TryGetValue(string testMethod, out IConfigurationReader configurationReader);
        void Dispose(string testMethod);
        void Dispose();
    }
}