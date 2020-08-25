using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebTestMethodManager : TestMethodManager, IWebTestMethodManager
    {
        public WebTestMethodManager(ILoggingUtility loggingUtility, TestContext testContext, IConfiguration appConfiguration, IFactory<IDriverCleanup> driverCleanup, IWebDriverService webDriverService): base (testContext, appConfiguration, loggingUtility)
        {
            WebDriverService = webDriverService;
            LoggingUtility = loggingUtility;
            DriverCleanup = driverCleanup;
        }

        public IWebDriverService WebDriverService { get; set; }
        public IFactory<IDriverCleanup> DriverCleanup { get; set; }

        public override ITestMethodBase Add(string testMethod)
        {
            CheckTestAlreadyInitialised(testMethod);

            IConfigurationReader configurationReader = new ConfigurationReader(TestContext, AppConfiguration);

            var webTestMethod = new WebTestMethod(DriverCleanup, LoggingUtility, configurationReader, testMethod);

            var browser = WebDriverService.StartWebDriver(testMethod);

            webTestMethod.WebDriver = browser.Item1;
            webTestMethod.WebDriverWait = browser.Item2;

            TestMethods.AddOrUpdate(testMethod, webTestMethod, (key, oldValue) => webTestMethod);

            return webTestMethod;
        }

        public override void Dispose(string testMethod)
        {
            TestMethods.TryGetValue(testMethod, out var testMethodCore);

            var webTestMethod = (WebTestMethod) testMethodCore;

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