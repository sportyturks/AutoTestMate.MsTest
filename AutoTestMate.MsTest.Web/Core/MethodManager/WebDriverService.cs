using System;
using System.Collections.Concurrent;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using NLog.Config;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebDriverService : IWebDriverService
    {
        public WebDriverService(ILoggingUtility loggingUtility, IFactory<IDriverCleanup> driverCleanup, IFactory<IWebDriver> webDriverFactory, IConfigurationReader configurationReader)
        {
            DriverCleanup = driverCleanup;
            WebDriverFactory = webDriverFactory;
            ConfigurationReader = configurationReader;
            LoggingUtility = loggingUtility;
        }
        
        public IFactory<IDriverCleanup> DriverCleanup { get; set; }
        public IConfigurationReader ConfigurationReader { get; set; }
        public IFactory<IWebDriver> WebDriverFactory { get; set; }
        public ILoggingUtility LoggingUtility { get; set; }


        public (IWebDriver, WebDriverWait) StartWebDriver(string testMethod)
        {
            var driverCleanup = DriverCleanup.Create();
            driverCleanup.Initialise();
            var browser = WebDriverFactory.Create();
            var loginWaitTIme = ConfigurationReader.GetConfigurationValue("LoginWaitTime");
            var browserWait = new WebDriverWait(browser, TimeSpan.FromMinutes(string.IsNullOrWhiteSpace(loginWaitTIme) ? 1 : Convert.ToInt32(loginWaitTIme) ));
            return (browser, browserWait);
        } 
    }
}