using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public interface IWebDriverService
    {
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        IConfigurationReader ConfigurationReader { get; set; }
        //IFactory<IWebDriver> WebDriverFactory { get; set; }
        ILoggingUtility LoggingUtility { get; set; }
        //(IWebDriver, WebDriverWait) StartWebDriver(string testMethod);
    }
}