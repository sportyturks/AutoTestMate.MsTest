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
        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected BasePageContainer(ITestMethodManager testMethodManager, TestContext testContext)
        {
            var webTestMethodManager = (WebTestMethodManager)testMethodManager;
            var webTestMethod = (WebTestMethod)webTestMethodManager.TryGetValue(testContext.TestName);

            Driver = webTestMethod?.WebDriver;
            ConfigurationReader = webTestMethod?.ConfigurationReader;
            LoggingUtility = webTestMethodManager.LoggingUtility;
        }

        public IWebDriver Driver { get; }

        public IConfigurationReader ConfigurationReader { get; }

        public ILoggingUtility LoggingUtility { get; }
    }
}