using System;
using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Core.MethodManager;
using Castle.MicroKernel;

namespace AutoTestMate.MsTest.Playwright.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class PlaywrightBasePage
    {
        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected PlaywrightBasePage(string testMethod)
        {
            TestMethod = testMethod;
            PlaywrightTestManager = PlaywrightTestManager.Instance;
            PlaywrightTestMethodManager = PlaywrightTestManager.PlaywrightTestMethodManager;
            PlaywrightTestMethod = (PlaywrightTestMethod)PlaywrightTestMethodManager.TryGetValue(testMethod);
            PlaywrightDriver = PlaywrightTestMethod?.PlaywrightDriver;
            ConfigurationReader = PlaywrightTestMethod?.ConfigurationReader;
            LoggingUtility = PlaywrightTestMethodManager.LoggingUtility;
            
            var timeout = ConfigurationReader?.GetConfigurationValue(Configuration.TimeoutKey);
            var timeoutValue = string.IsNullOrWhiteSpace(timeout) ? Configuration.DefaultTimeoutValue : Convert.ToInt64(timeout);
        }

        protected IPlaywrightDriver PlaywrightDriver { get; }
    
        protected IConfigurationReader ConfigurationReader { get; }
      
        protected ILoggingUtility LoggingUtility  { get; }
        
        protected string TestMethod  { get; }
        
        protected PlaywrightTestMethodManager PlaywrightTestMethodManager  { get; }
        
        protected PlaywrightTestManager PlaywrightTestManager  { get; }
        
        protected PlaywrightTestMethod PlaywrightTestMethod  { get; }

        protected T GetPage<T>()
        {
            return PlaywrightTestManager.Container.Resolve<T>(new Arguments { { "testName", TestMethod } });
        }
    }
}