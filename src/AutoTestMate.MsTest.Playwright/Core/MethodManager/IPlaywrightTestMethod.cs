using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public interface IPlaywrightTestMethod
    {
        IWebDriverService WebDriverService { get; set; }
        //IWebDriver WebDriver { get; set; }
        //WebDriverWait WebDriverWait { get; set; }
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        void StartWebDriver();
    }
}