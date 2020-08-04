using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class TestMethodManager : ITestMethodManager
    {
        public TestMethodManager(TestContext testContext, IConfiguration appConfiguration, IConfigurationService configurationService, ITestInitialiseService testInitialiseService)
        {
            TestContext = testContext;
            AppConfiguration = appConfiguration;
            ConfigurationService = configurationService;
            TestInitialiseService = testInitialiseService;

        }
        public List<string> TestMethods { get; set; }
        public TestContext TestContext { get; set; }
        public IConfigurationService ConfigurationService { get; set; }
        public IConfiguration AppConfiguration { get; set; }
        public ITestInitialiseService TestInitialiseService { get; set; }
        public void CheckTestAlreadyInitialised(string testMethod)
        {
            TestInitialiseService.CheckTestAlreadyInitialised(testMethod);
        }
    }
}