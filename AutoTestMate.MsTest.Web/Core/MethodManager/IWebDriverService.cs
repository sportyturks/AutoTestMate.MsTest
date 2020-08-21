using System.Collections.Concurrent;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public interface IWebDriverService
    {
        ConcurrentDictionary<string, IWebDriver> WebDrivers { get; set; }
        ConcurrentDictionary<string, WebDriverWait> WebDriverWaitList { get; set; }
        ITestInitialiseService TestInitialiseService { get; set; }
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        IConfigurationReader ConfigurationReader { get; set; }
        IFactory<IWebDriver> WebDriverFactory { get; set; }
        ILoggingUtility LoggingUtility { get; set; }
        void AddOrUpdate(string testName, IWebDriver webDriver, WebDriverWait webDriverWait);
        void StartWebDriver(string testMethod);
        void Dispose(string testMethod);
        bool TryGetValue(string testMethod, out IWebDriver webDriver);
        bool TryGetValue(string testMethod, out WebDriverWait webDriverWait);
    }
}