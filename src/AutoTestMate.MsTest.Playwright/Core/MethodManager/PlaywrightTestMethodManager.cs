using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Playwright.Core.MethodManager
{
    public class PlaywrightTestMethodManager : TestMethodManager, IPlaywrightTestMethodManager
    {
        public PlaywrightTestMethodManager(ILoggingUtility loggingUtility, TestContext testContext, IConfiguration appConfiguration, IFactory<IDriverCleanup> driverCleanup): base (testContext, appConfiguration, loggingUtility)
        {
            LoggingUtility = loggingUtility;
            DriverCleanup = driverCleanup;
        }

        public IFactory<IDriverCleanup> DriverCleanup { get; set; }

        public override ITestMethodBase Add(string testMethod)
        {
            CheckTestAlreadyInitialised(testMethod);

            IConfigurationReader configurationReader = new ConfigurationReader(TestContext, AppConfiguration);

            //var driver = PlaywrightTestManager.Instance.Container.Resolve<IPlaywrightDriver>();
            var driver = new PlaywrightDriver(LoggingUtility, configurationReader);

            var playwrightTestMethod = new PlaywrightTestMethod(DriverCleanup, LoggingUtility, configurationReader, driver, testMethod);
            
            AsyncHelper.RunSync( ()=> playwrightTestMethod.StartWebDriver());
            
            TestMethods.AddOrUpdate(testMethod, playwrightTestMethod, (key, oldValue) => playwrightTestMethod);
            
            return playwrightTestMethod;
        }

        public override void Dispose(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            var playwrightTestMethod = (PlaywrightTestMethod) testMethodCore;

            if (playwrightTestMethod == null) return;

            playwrightTestMethod.Dispose();

            TestMethods.AddOrUpdate(testMethod, playwrightTestMethod, (key, oldValue) => playwrightTestMethod);

            TestMethods.TryRemove(testMethod, out _);
        }

        public override void Dispose()
        {
            TestMethods.Clear();
        }
    }
}