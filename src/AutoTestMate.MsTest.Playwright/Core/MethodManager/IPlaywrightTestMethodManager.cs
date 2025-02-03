using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public interface IPlaywrightTestMethodManager: ITestMethodManager
    {
        new ILoggingUtility LoggingUtility { get; set; }
    }
}