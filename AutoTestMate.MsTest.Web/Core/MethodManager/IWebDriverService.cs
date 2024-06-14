using AutoTestMate.MsTest.Infrastructure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public interface IWebDriverService
    {
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        IConfigurationReader ConfigurationReader { get; set; }
        IFactory<IWebDriver> WebDriverFactory { get; set; }
        ILoggingUtility LoggingUtility { get; set; }
        (IWebDriver, WebDriverWait) StartWebDriver(string testMethod);
    }
}