using System.Threading;
using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Infrastructure.Helpers;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public class PlaywrightTestMethod(
        IFactory<IDriverCleanup> driverCleanup,
        ILoggingUtility loggingUtility,
        IConfigurationReader configurationReader,
        IPlaywrightDriver playwrightDriver,
        string testMethod)
        : TestMethodBase(loggingUtility, configurationReader, testMethod), IPlaywrightTestMethod
    {
        public IPlaywrightDriver  PlaywrightDriver { get; set; } = playwrightDriver;
        public IFactory<IDriverCleanup> DriverCleanup { get; set; } = driverCleanup;

        public async Task StartWebDriver()
        {
            await PlaywrightDriver.StartPlaywright().ConfigureAwait(false);
            
            IsInitialised = true;
        }

        public override void Dispose()
        {
            try
            {
                AsyncHelper.RunSync(() => PlaywrightDriver?.Dispose());
            }
            catch (System.Exception exp)
            {
                LoggingUtility.Error(exp.Message);
            }
            finally
            {
                if (IsInitialised)
                {
                    var driverCleanup = DriverCleanup.Create();
                    driverCleanup.Dispose();
                }

                IsInitialised = false;
            }
        }
    }
}
