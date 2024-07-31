using System;
using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Core.MethodManager;
using Castle.MicroKernel;

namespace AutoTestMate.MsTest.Playwright.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class BasePage
    {
        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected BasePage(string testMethod)
        {
            TestMethod = testMethod;
            PlaywrightTestManager = PlaywrightTestManager.Instance;
            PlaywrightTestMethodManager = PlaywrightTestManager.PlaywrightTestMethodManager;
            PlaywrightTestMethod = (PlaywrightTestMethod)PlaywrightTestMethodManager.TryGetValue(testMethod);
            //Driver = PlaywrightTestMethod?.WebDriver;
            ConfigurationReader = PlaywrightTestMethod?.ConfigurationReader;
            LoggingUtility = PlaywrightTestMethodManager.LoggingUtility;
            
            var timeout = ConfigurationReader.GetConfigurationValue(Configuration.TimeoutKey);
            var timeoutValue = string.IsNullOrWhiteSpace(timeout) ? Configuration.DefaultTimeoutValue : Convert.ToInt64(timeout);
            //Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutValue);
        }

        //protected IWebDriver Driver { get; }
    
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