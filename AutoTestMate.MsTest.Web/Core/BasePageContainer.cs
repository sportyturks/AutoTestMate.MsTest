using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace AutoTestMate.MsTest.Web.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class BasePageContainer
    {
        private readonly IWebDriver _driver;
        private readonly IConfigurationReader _configurationReader;
        private readonly ILoggingUtility _loggingUtility;

        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected BasePageContainer(ITestMethodManager testMethodManager, TestContext testContext)
        {
            var webTestMethodManager = (WebTestMethodManager)testMethodManager;
            var webTestMethod = (WebTestMethod)webTestMethodManager.TryGetValue(testContext.TestName);

            _driver = webTestMethod?.WebDriver;
            _configurationReader = webTestMethod?.ConfigurationReader;
            _loggingUtility = webTestMethodManager.LoggingUtility;
        }

        public IWebDriver Driver => _driver;

        public IConfigurationReader ConfigurationReader => _configurationReader;

        public ILoggingUtility LoggingUtility => _loggingUtility;
    }
}