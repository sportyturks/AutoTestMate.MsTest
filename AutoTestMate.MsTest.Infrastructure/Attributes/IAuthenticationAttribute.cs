using AutoTestMate.MsTest.Infrastructure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Attributes
{
    public interface IAuthenticationAttribute
    {
        void BeforeTest(TestContext testContext, ITestManager testManager);
        void AfterTest(ITestManager testManager);
    }
}
