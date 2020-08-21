using System;
using System.Collections.Concurrent;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using NLog.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebDriverService : IWebDriverService, IDisposable
    {
        public WebDriverService(ILoggingUtility loggingUtility, IFactory<IDriverCleanup> driverCleanup,
            IFactory<IWebDriver> webDriverFactory, IConfigurationReader configurationReader, ITestInitialiseService testInitialiseService)
        {
            
        }
        
        public ConcurrentDictionary<string, IWebDriver> WebDrivers { get; set; }
        public ConcurrentDictionary<string, WebDriverWait> WebDriverWaitList { get; set; }
        public ITestInitialiseService TestInitialiseService { get; set; }
        public IFactory<DriverCleanup> DriverCleanup { get; set; }
        public IConfigurationReader ConfigurationReader { get; set; }
        public IFactory<IWebDriver> WebDriverFactory { get; set; }
        public ILoggingUtility LoggingUtility { get; set; }

        public void AddOrUpdate(string testName, IWebDriver webDriver, WebDriverWait webDriverWait)
        {
            WebDrivers.AddOrUpdate(testName, webDriver, (key, oldValue) => webDriver);
            WebDriverWaitList.AddOrUpdate(testName, webDriverWait, (key, oldValue) => webDriverWait);
        }

        public void StartWebDriver(string testMethod)
        {
            var driverCleanup = DriverCleanup.Create();
            driverCleanup.Initialise();
            var browser = WebDriverFactory.Create();
            var loginWaitTIme = ConfigurationReader.GetConfigurationValue("LoginWaitTime");
            var browserWait = new WebDriverWait(browser, TimeSpan.FromMinutes(string.IsNullOrWhiteSpace(loginWaitTIme) ? 1 : Convert.ToInt32(loginWaitTIme) ));
            AddOrUpdate(testMethod, browser, browserWait);
            TestInitialiseService.AddOrUpdate(testMethod, true);
        }

        public void Dispose(string testMethod)
        {
            try
            {
                var testWebDriverExists = WebDrivers.TryGetValue(testMethod, out var testWebDriver);
                
                if (testWebDriverExists)
                {
                    testWebDriver?.Quit();
                    //testWebDriver?.Dispose();
                }
                
                var testWebDriverWaitExists = WebDriverWaitList.TryGetValue(testMethod, out var testWebDriverWait);
                if (testWebDriverWaitExists)
                {
                    testWebDriverWait = null;
                }
            }
            catch (System.Exception exp)
            {
                LoggingUtility.Error(exp.Message);
            }
            finally
            {
                TestInitialiseService.TryGetValue(testMethod, out var isInitialised);
                
                if (isInitialised)
                {
                    var testWebDriverExists = WebDrivers.TryGetValue(testMethod, out var testWebDriver);
                    if (testWebDriverExists)
                    {
                        testWebDriver?.Dispose();
                    }
                    var driverCleanup = DriverCleanup.Create();
                    driverCleanup.Dispose();
                }
                
                WebDriverWaitList.TryRemove(testMethod, out _);
                WebDrivers.TryRemove(testMethod, out _);
                TestInitialiseService.AddOrUpdate(testMethod, false);
            }
        }

        public bool TryGetValue(string testMethod, out IWebDriver webDriver)
        {
            return WebDrivers.TryGetValue(testMethod, out webDriver);
        }
        
        public bool TryGetValue(string testMethod, out WebDriverWait webDriverWait)
        {
            return WebDriverWaitList.TryGetValue(testMethod, out webDriverWait);
        }

        public void Dispose()
        {
            foreach (var webDriver in WebDrivers)
            {
                webDriver.Value?.Quit();
                webDriver.Value?.Dispose();
            }
        }
    }
}