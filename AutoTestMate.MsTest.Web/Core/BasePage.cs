using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using OpenQA.Selenium;

namespace AutoTestMate.MsTest.Web.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class BasePage
    {
        private readonly IWebDriver _driver;
        private readonly IConfigurationReader _configurationReader;
        private readonly ILoggingUtility _loggingUtility;

        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected BasePage(string testMethod, ITestMethodManager testMethodManager)
        {
            var webTestMethodManager = (WebTestMethodManager)testMethodManager;
            webTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
            var webTestMethod = (WebTestMethod)testMethodBase;

            _driver = webTestMethod?.WebDriver;
            _configurationReader = webTestMethod?.ConfigurationReader;
            _loggingUtility = webTestMethodManager.LoggingUtility;
        }

        public IWebDriver Driver => _driver;

        public IConfigurationReader ConfigurationReader => _configurationReader;

        public ILoggingUtility LoggingUtility => _loggingUtility;
    }
}