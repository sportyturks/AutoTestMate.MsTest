using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public interface IPlaywrightTestMethodManager: ITestMethodManager
    {
        IWebDriverService WebDriverService { get; set; }
        new ILoggingUtility LoggingUtility { get; set; }
    }
}