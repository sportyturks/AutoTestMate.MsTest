using System;
using AutoTestMate.MsTest.Infrastructure.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public abstract class TestBase : CustomTestAttributes
	{
		[TestInitialize]
		public virtual void OnTestInitialise()
		{
			try
			{
				TestManager = Core.TestManager.Instance();
				TestManager.OnTestMethodInitialise(TestMethod, TestContext);
				CustomAttributesInitialise();
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
            try
            {
                CustomAttributesCleanup();

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
                TestManager.OnTestCleanup();
            }
        }

        public virtual void HandleException(Exception exp)
		{
			if (TestManager.LoggingUtility == null || TestManager.ConfigurationReader == null) throw exp;

			LoggingUtility.Error(Exceptions.Exception.ExceptionMsgSetupError + exp.Message);
			TestContext.WriteLine(Exceptions.Exception.ExceptionMsgSetupError + exp.Message);

            throw exp;
		}

		public virtual IConfigurationReader ConfigurationReader => TestManager.ConfigurationReader;

		public virtual ILoggingUtility LoggingUtility => TestManager.LoggingUtility;

		public string TestMethod => TestContext.TestName;
	}
}