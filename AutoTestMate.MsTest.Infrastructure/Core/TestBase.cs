using System;
using System.Runtime.CompilerServices;
using AutoTestMate.MsTest.Infrastructure.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public abstract class TestBase : CustomTestAttributes
	{
		[TestInitialize]
		public virtual void OnTestInitialise()
		{
			var testMethod = TestContext.TestName;
			
			try
			{
				TestManager = Core.TestManager.Instance();
				TestManager.OnTestMethodInitialise(testMethod, TestContext);
				CustomAttributesInitialise(testMethod);
			}
			catch (Exception ex)
			{
				if (LoggingUtility == null || ConfigurationReader == null) throw;

				LoggingUtility.Error(Exceptions.Exception.ExceptionMsgSetupError + ex.Message);

				throw;
			}
		}

        [TestCleanup]
        public virtual void OnTestCleanup()
        {
	        var testMethod = TestContext.TestName;
	        
            try
            {
	            
                CustomAttributesCleanup(testMethod);
                TestManager.Dispose(testMethod);

                if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
                {
                    try
                    {
                        TestContext.WriteLine(Constants.Generic.TestErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
                else
                {
                    TestContext.WriteLine(Constants.Generic.TestSuccessMessage);
                }
            }
            catch (Exception exp)
            {
                HandleException(exp);
            }
            finally
            {
                TestManager.OnTestCleanup(testMethod);
            }
        }

        public virtual void HandleException(Exception exp)
		{
			if (TestManager.LoggingUtility == null || TestManager.ConfigurationReader == null) throw exp;

			LoggingUtility.Error(Exceptions.Exception.ExceptionMsgSetupError + exp.Message);
			TestContext.WriteLine(Exceptions.Exception.ExceptionMsgSetupError + exp.Message);

            throw exp;
		}

		public virtual IConfigurationReader ConfigurationReader => TestManager.TestMethodManager.TryGetValue(TestMethod).ConfigurationReader;

        public virtual ILoggingUtility LoggingUtility => TestManager.LoggingUtility;

		public string TestMethod => TestContext.TestName;

        public virtual IConfigurationReader GetConfigurationReader([CallerMemberName] string testName = null)
        {
            return TestManager.TestMethodManager.TryGetValue(testName).ConfigurationReader;
        }
	}
}