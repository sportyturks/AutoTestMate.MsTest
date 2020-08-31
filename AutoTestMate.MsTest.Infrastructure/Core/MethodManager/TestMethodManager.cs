using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog.Fluent;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public class TestMethodManager : ITestMethodManager
    {
        public TestMethodManager(TestContext testContext, IConfiguration appConfiguration, ILoggingUtility loggingUtility)
        {
            TestContext = testContext;
            AppConfiguration = appConfiguration;
            LoggingUtility = loggingUtility;
            TestMethods = new ConcurrentDictionary<string, ITestMethodBase>();
        }

        public ConcurrentDictionary<string, ITestMethodBase> TestMethods { get; set; }
        public  TestContext TestContext { get; set; }
        public  ILoggingUtility LoggingUtility { get; set; }
        public  IConfiguration AppConfiguration { get; set; }

        public virtual void CheckTestAlreadyInitialised(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodMain);

            if (testMethodMain == null) return; 

            if (testMethodMain.IsInitialised) throw new ApplicationException($"Test Method in [{testMethod}], has already been initialised");
        }

        public virtual ITestMethodBase Add(string testMethod)
        {
            CheckTestAlreadyInitialised(testMethod);

            IConfigurationReader configurationReader = new ConfigurationReader(TestContext,AppConfiguration);

            ITestMethodBase testMethodMain = new TestMethodCore(LoggingUtility, configurationReader, testMethod);
            
            TestMethods.AddOrUpdate(testMethod, testMethodMain, (key, oldValue) => testMethodMain);

            return testMethodMain;
        }

        public virtual ITestMethodBase InitialiseTestMethod(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            if (testMethodCore == null) throw new ApplicationException($"Test Method {testMethod} Doesn't Exist");

            testMethodCore.IsInitialised = true;

            TestMethods.AddOrUpdate(testMethod, testMethodCore, (key,  oldValue) => testMethodCore);

            return testMethodCore;
        }

        public virtual ITestMethodBase UpdateConfigurationReader(string testMethod, IConfigurationReader configurationReader)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            if (testMethodCore == null) throw new ApplicationException($"Test Method {testMethod} Doesn't Exist");

            testMethodCore.ConfigurationReader = configurationReader;

            TestMethods.AddOrUpdate(testMethod, testMethodCore, (key, oldValue) => testMethodCore);

            return testMethodCore;
        }

        public virtual ITestMethodBase TryGetValue(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodMain);

            return testMethodMain;
        }

        public virtual void Dispose(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            if (testMethodCore == null) return;

            testMethodCore.IsInitialised = false;

            TestMethods.AddOrUpdate(testMethod, testMethodCore, (key, oldValue) => testMethodCore);

            TestMethods.TryRemove(testMethod, out _);
        }

        public virtual void Dispose()
        {
            TestMethods.Clear();
        }
    }
}