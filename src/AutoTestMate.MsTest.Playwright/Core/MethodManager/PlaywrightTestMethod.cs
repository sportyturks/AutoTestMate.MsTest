using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public class PlaywrightTestMethod : TestMethodBase, IPlaywrightTestMethod
    {
        public PlaywrightTestMethod(IFactory<IDriverCleanup> driverCleanup,
            ILoggingUtility loggingUtility,
            IConfigurationReader configurationReader,
            IPlaywrightDriver playwrightDriver,
            string testMethod) : base(loggingUtility, configurationReader, testMethod)
        {
            PlaywrightDriver = playwrightDriver;
            DriverCleanup = driverCleanup;
        }

        public IPlaywrightDriver  PlaywrightDriver { get; set; }
        public IFactory<IDriverCleanup> DriverCleanup { get; set; }

        public async Task StartWebDriver()
        {
            await PlaywrightDriver.StartPlaywright().ConfigureAwait(false);
        }

        public override void Dispose()
        {
            try
            {
                PlaywrightDriver?.Dispose();
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
