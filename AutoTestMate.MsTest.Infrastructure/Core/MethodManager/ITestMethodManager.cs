using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public interface ITestMethodManager
    {
        IConfigurationService ConfigurationService { get; set; }
        ITestInitialiseService TestInitialiseService { get; set; }
        List<string> TestMethods { get; set; }
        TestContext TestContext { get; set; }
         IConfiguration AppConfiguration { get; set; }
        void CheckTestAlreadyInitialised(string testMethod);
    }
}