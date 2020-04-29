using System.Net.Http;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace AutoTestMate.MsTest.Web.Core
{
	public abstract class WebTestBase : ServiceTestBase
	{
		[TestInitialize]
		public override void OnTestInitialise()
		{
			try
			{
				TestManager = WebTestManager.Instance();
				TestManager.OnTestMethodInitialise(TestContext);
				CustomAttributesInitialise();
			}
			catch (System.Exception ex)
			{
				if (LoggingUtility == null || ConfigurationReader == null) throw;

				LoggingUtility.Error(Constants.Exceptions.ExceptionMsgSetupError + ex.Message);

                throw;
            }
		}

		[TestCleanup]
		public override void OnTestCleanup()
		{
			try
			{
				CustomAttributesCleanup();

				if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
				{
					TestContext.WriteLine(Infrastructure.Constants.Generic.TestErrorMessage);

					if (LoggingUtility == null || ConfigurationReader == null) return;

					var outputPath = !string.IsNullOrWhiteSpace(ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)) ? $"{ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)}" : $"{TestContext.TestRunResultsDirectory}{Constants.Configuration.ScreenshotsDirectory}";

					if (((WebTestManager)TestManager).IsDriverNull) return;

					TestContext.WriteLine($"Attempting to capture screenshot to: {outputPath}");

					var captureScreenShot = Driver.ScreenShotSaveFile(outputPath, TestContext.TestName);

					if (string.IsNullOrWhiteSpace(captureScreenShot)) return;

					TestContext.AddResultFile(captureScreenShot);

					LoggingUtility.Error(captureScreenShot);
				}
				else
				{
					TestContext.WriteLine(Infrastructure.Constants.Generic.TestSuccessMessage);
				}
			}
			catch (System.Exception ex)
			{
				HandleException(ex);
			}
			finally
			{
				TestManager.OnTestCleanup();
			}
		}

		public IWebDriver Driver => ((WebTestManager)TestManager).Browser;

		public override HttpClient HttpClient => ((WebTestManager)TestManager).HttpClient;
    }
}




