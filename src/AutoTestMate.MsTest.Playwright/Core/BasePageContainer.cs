using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Playwright.Core.MethodManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Playwright.Core
{
    [ExcludeFromCodeCoverage]
    public abstract class PlaywrightBasePageContainer
    {
        /// <summary>
        /// Constructor of Base Page
        /// </summary>
        protected PlaywrightBasePageContainer(ITestMethodManager testMethodManager, TestContext testContext)
        {
            var playwrightTestMethodManager = (PlaywrightTestMethodManager)testMethodManager;
            var playwrightTestMethod = (PlaywrightTestMethod)playwrightTestMethodManager.TryGetValue(testContext.TestName);

            PlaywrightDriver = playwrightTestMethod?.PlaywrightDriver;
            ConfigurationReader = playwrightTestMethod?.ConfigurationReader;
            LoggingUtility = playwrightTestMethod?.LoggingUtility;
        }

        public IPlaywrightDriver PlaywrightDriver { get; }

        public IConfigurationReader ConfigurationReader { get; }

        public ILoggingUtility LoggingUtility { get; }
    }
}