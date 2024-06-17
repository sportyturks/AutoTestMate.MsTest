using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using Castle.Facilities.Startable;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebTestMethod : TestMethodBase, IWebTestMethod
    {
        public WebTestMethod(IFactory<IDriverCleanup> driverCleanup,
            ILoggingUtility loggingUtility,
            IConfigurationReader configurationReader,
            IWebDriverService webDriverService,
            string testMethod) : base(loggingUtility, configurationReader, testMethod)
        {
            WebDriverService = webDriverService;
            DriverCleanup = driverCleanup;
            StartWebDriver();
        }

        public IWebDriver  WebDriver { get; set; }
        public IWebDriverService WebDriverService { get; set; }
        public WebDriverWait WebDriverWait { get; set; }
        public IFactory<IDriverCleanup> DriverCleanup { get; set; }

        public void StartWebDriver()
        {
            if (WebDriverService != null)
            {
                var browser = WebDriverService.StartWebDriver(TestMethod);

                WebDriver = browser.Item1;
                WebDriverWait = browser.Item2;
            }
        }

        public override void Dispose()
        {
            try
            {
                var testWebDriverExists = WebDriver != null;
                if (testWebDriverExists)
                {
                    WebDriver?.Quit();
                }

                WebDriverWait = null;
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
