using AutoTestMate.MsTest.Infrastructure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public interface IWebTestMethod
    {
        IWebDriverService WebDriverService { get; set; }
        IWebDriver WebDriver { get; set; }
        WebDriverWait WebDriverWait { get; set; }
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        void StartWebDriver();
    }
}