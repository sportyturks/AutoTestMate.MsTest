using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Web.Core.MethodManager;
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
        protected BasePageContainer(IWebTestMethodManager webTestMethodManager)
        {
            var testMethod = webTestMethodManager.TestContext.TestName;
            webTestMethodManager.WebDriverService.TryGetValue(testMethod, out IWebDriver driver);
            webTestMethodManager.ConfigurationService.TryGetValue(testMethod, out IConfigurationReader configurationReader);
            
            _driver = driver;
            _configurationReader = configurationReader;
            _loggingUtility = webTestMethodManager.LoggingUtility;
        }

        public IWebDriver Driver => _driver;

        public IConfigurationReader ConfigurationReader => _configurationReader;

        public ILoggingUtility LoggingUtility => _loggingUtility;
    }
}