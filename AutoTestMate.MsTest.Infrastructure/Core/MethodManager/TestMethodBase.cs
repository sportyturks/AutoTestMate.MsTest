using System;
using System.Collections.Generic;
using System.Text;
using DocumentFormat.OpenXml.Presentation;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public abstract class TestMethodBase(
        ILoggingUtility loggingUtility,
        IConfigurationReader configurationReader,
        string testMethod)
        : ITestMethodBase, IDisposable
    {
        public string TestMethod { get; set; } = testMethod;
        public bool IsInitialised { get; set; } = false;
        public IConfigurationReader ConfigurationReader { get; set; } = configurationReader;
        public ILoggingUtility LoggingUtility { get; set; } = loggingUtility;

        public virtual void Dispose()
        {
            IsInitialised = false;
        }
    }
}
