using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public interface IWebTestMethodManager: ITestMethodManager
    {
        IWebDriverService WebDriverService { get; set; }
    }
}