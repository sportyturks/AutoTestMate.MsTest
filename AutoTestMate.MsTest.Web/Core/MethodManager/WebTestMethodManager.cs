using System.Collections.Generic;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebTestMethodManager : TestMethodManager, IWebTestMethodManager
    {
        public WebTestMethodManager(TestContext testContext, IConfiguration appConfiguration, IConfigurationService configurationService, ITestInitialiseService testInitialiseService, IWebDriverService webDriverService): base (testContext, appConfiguration, configurationService, testInitialiseService)
        {
            WebDriverService = webDriverService;
        }

        public IWebDriverService WebDriverService { get; set; }
    }
}