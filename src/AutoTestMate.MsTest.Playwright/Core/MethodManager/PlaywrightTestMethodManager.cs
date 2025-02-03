using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
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

            var driver = PlaywrightTestManager.Instance.Container.Resolve<IPlaywrightDriver>();

            var playwrightTestMethod = new PlaywrightTestMethod(DriverCleanup, LoggingUtility, configurationReader, driver, testMethod);
            
            TestMethods.AddOrUpdate(testMethod, playwrightTestMethod, (key, oldValue) => playwrightTestMethod);
            
            return playwrightTestMethod;
        }

        public override void Dispose(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            var webTestMethod = (PlaywrightTestMethod) testMethodCore;

            if (webTestMethod == null) return;

            webTestMethod.Dispose();

            TestMethods.AddOrUpdate(testMethod, webTestMethod, (key, oldValue) => webTestMethod);

            TestMethods.TryRemove(testMethod, out _);
        }

        public override void Dispose()
        {
            TestMethods.Clear();
        }
    }
}