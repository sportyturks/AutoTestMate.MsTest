using System;
using AutoTestMate.MsTest.Infrastructure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public sealed class AuthenticationAttribute : Attribute, IAuthenticationAttribute
    {
        public void BeforeTest(TestContext testContext, ITestManager testManager)
        {
            Console.WriteLine("Before Test Called");
        }

        public void AfterTest(ITestManager testManager)
        {
			Console.WriteLine("After Test Called");
		}
    }
}
