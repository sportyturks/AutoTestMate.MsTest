using System;
using System.Net.Http;
using AutoTestMate.MsTest.Infrastructure.Core;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Services.Core
{
    public class ServiceTestBase : TestBase
    {
        #region Initilise/Cleanup

        [TestInitialize]
        public override void OnTestInitialise()
        {
            var testMethod = TestContext.TestName;
            try
            {
                TestManager = ServiceTestManager.Instance();
                TestManager.OnTestMethodInitialise(testMethod,TestContext);
                CustomAttributesInitialise(testMethod);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }
        
        [TestCleanup]
        public override void OnTestCleanup()
        {
            base.OnTestCleanup();
        }

        #endregion
        
        public virtual HttpClient HttpClient => ((ServiceTestManager)TestManager).HttpClient;
    }
}