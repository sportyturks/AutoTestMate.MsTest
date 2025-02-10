using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public interface IPlaywrightTestMethod
    {
        IPlaywrightDriver PlaywrightDriver { get; set; }
        IFactory<IDriverCleanup> DriverCleanup { get; set; }
        Task StartWebDriver();
    }
}