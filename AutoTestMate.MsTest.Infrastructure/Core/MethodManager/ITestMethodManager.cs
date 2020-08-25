using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public interface ITestMethodManager
    {
        ConcurrentDictionary<string, ITestMethodBase> TestMethods { get; set; }
        TestContext TestContext { get; set; }
        ILoggingUtility LoggingUtility { get; set; }
        IConfiguration AppConfiguration { get; set; }
        void CheckTestAlreadyInitialised(string testMethod);
        ITestMethodBase Add(string testMethod);
        ITestMethodBase InitialiseTestMethod(string testMethod);
        ITestMethodBase UpdateConfigurationReader(string testMethod, IConfigurationReader configurationReader);
        ITestMethodBase TryGetValue(string testMethod);
        void Dispose(string testMethod);
        void Dispose();
    }
}