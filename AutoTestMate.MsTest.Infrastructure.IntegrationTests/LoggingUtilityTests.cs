using System;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Web.Core;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using AutoTestMate.MsTest.Web.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.IntegrationTests
{
    [TestClass]
    public class LoggingUtilityTests: WebTestBase
    {
        [TestMethod]
        public void EnsureLoggingFileCreated()
        {
            LoggingUtility.Info("This is an info message");
            LoggingUtility.Error("This is an info message");
        }
        [TestMethod]
        public void EnsureScreenshotCreated()
        {
            LoggingUtility.Info("This is an info message");
            LoggingUtility.Error("This is an info message");
            
            CaptureScreenshot();
        }
        
    }
}