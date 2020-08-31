using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Presentation;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public abstract class TestMethodBase : ITestMethodBase, IDisposable
    {
        protected TestMethodBase(ILoggingUtility loggingUtility, IConfigurationReader configurationReader, string testMethod)
        {
            IsInitialised = false;
            LoggingUtility = loggingUtility;
            ConfigurationReader = configurationReader;
            TestMethod = testMethod;
        }

        public string TestMethod { get; set; }
        public bool IsInitialised { get; set; }
        public IConfigurationReader ConfigurationReader { get; set; }
        public ILoggingUtility LoggingUtility { get; set; }

        public virtual void Dispose()
        {
            IsInitialised = false;
        }
    }
}
