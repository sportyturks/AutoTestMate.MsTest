using System;
using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Web.Constants;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using Castle.MicroKernel;
using OpenQA.Selenium;

namespace AutoTestMate.MsTest.Web.Core
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
            WebTestManager = WebTestManager.Instance();
            WebTestMethodManager = WebTestManager.WebTestMethodManager;
            WebTestMethod = (WebTestMethod)WebTestMethodManager.TryGetValue(testMethod);
            Driver = WebTestMethod?.WebDriver;
            ConfigurationReader = WebTestMethod?.ConfigurationReader;
            LoggingUtility = WebTestMethodManager.LoggingUtility;
            
            var timeout = ConfigurationReader.GetConfigurationValue(Configuration.TimeoutKey);
            var timeoutValue = string.IsNullOrWhiteSpace(timeout) ? Configuration.DefaultTimeoutValue : Convert.ToInt64(timeout);
            Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(timeoutValue);
        }

        protected IWebDriver Driver { get; }
    
        protected IConfigurationReader ConfigurationReader { get; }
      
        protected ILoggingUtility LoggingUtility  { get; }
        
        protected string TestMethod  { get; }
        
        protected WebTestMethodManager WebTestMethodManager  { get; }
        
        protected WebTestManager WebTestManager  { get; }
        
        protected WebTestMethod WebTestMethod  { get; }

        protected T GetPage<T>()
        {
            return WebTestManager.Container.Resolve<T>(new Arguments { { "testName", TestMethod } });
        }
    }
}