using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.IntegrationTests
{
    [TestClass]
    public class AssemblyInitialise
    {
        [AssemblyInitialize]
        public static void Initialise(TestContext testContext)
        {
            WebTestManager.Instance.OnInitialiseAssemblyDependencies(testContext);
        }

        [AssemblyCleanup]
        public static void Cleanup()
        {
            WebTestManager.Instance.OnDisposeAssemblyDependencies();
        }
    }
}