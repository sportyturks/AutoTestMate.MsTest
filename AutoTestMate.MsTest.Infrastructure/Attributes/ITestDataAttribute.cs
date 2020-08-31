using AutoTestMate.MsTest.Infrastructure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Attributes
{
    public interface ITestDataAttribute
    {
        void BeforeTest(string testMethod, TestContext testContext, ITestManager testManager);
        void AfterTest(string testMethod, ITestManager testManager);
    }
}