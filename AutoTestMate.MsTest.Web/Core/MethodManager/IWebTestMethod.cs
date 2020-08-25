using AutoTestMate.MsTest.Infrastructure.Core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public interface IWebTestMethod
    {
        IWebDriver WebDriver { get; set; }
        WebDriverWait WebDriverWait { get; set; }
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
    }
}