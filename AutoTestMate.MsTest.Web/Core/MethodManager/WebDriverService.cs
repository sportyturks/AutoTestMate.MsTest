using System;
using AutoTestMate.MsTest.Infrastructure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebDriverService(
        ILoggingUtility loggingUtility,
        IFactory<IDriverCleanup> driverCleanup,
        IFactory<IWebDriver> webDriverFactory,
        IConfigurationReader configurationReader)
        : IWebDriverService
    {
        public IFactory<IDriverCleanup> DriverCleanup { get; set; } = driverCleanup;
        public IConfigurationReader ConfigurationReader { get; set; } = configurationReader;
        public IFactory<IWebDriver> WebDriverFactory { get; set; } = webDriverFactory;
        public ILoggingUtility LoggingUtility { get; set; } = loggingUtility;
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