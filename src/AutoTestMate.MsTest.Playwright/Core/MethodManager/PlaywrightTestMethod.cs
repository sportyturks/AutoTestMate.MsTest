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
        public const int DriverWaitTime = 1500;
        public IPlaywrightDriver  PlaywrightDriver { get; set; } = playwrightDriver;
        public IFactory<IDriverCleanup> DriverCleanup { get; set; } = driverCleanup;

        public async Task StartWebDriver()
        {
            await PlaywrightDriver.StartPlaywright().ConfigureAwait(false);
            //Playwright Driver seems to take too short to create and all
            Thread.Sleep(DriverWaitTime);
            
            IsInitialised = true;
        }

        public override void Dispose()
        {
            try
            {
                AsyncHelper.RunSync(() => PlaywrightDriver?.Dispose());
                Thread.Sleep(DriverWaitTime);
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
