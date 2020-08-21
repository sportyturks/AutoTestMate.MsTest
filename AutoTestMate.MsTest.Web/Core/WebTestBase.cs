using System.Net.Http;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core
{
	public abstract class WebTestBase : ServiceTestBase
	{
		[TestInitialize]
		public override void OnTestInitialise()
		{
			var testMethod = TestMethod;
			
			try
			{
				TestManager = WebTestManager.Instance();
				TestManager.OnTestMethodInitialise(testMethod, TestContext);
				CustomAttributesInitialise(testMethod);
			}
			catch (System.Exception ex)
            {
                var webTestManager = ((WebTestManager) TestManager);

				if (LoggingUtility == null || ConfigurationReader == null) throw;

				LoggingUtility.Error(Constants.Exceptions.ExceptionMsgSetupError + ex.Message);
				
				if (webTestManager.IsDriverNull(testMethod)) throw;

                webTestManager.WebTestMethodManager.WebDriverService.TryGetValue(testMethod, out IWebDriver driver);

				var outputScreenshotsDirectory = ConfigurationReader.GetConfigurationValue("OutputScreenshotsDirectory");

				var outputPath = !string.IsNullOrWhiteSpace(outputScreenshotsDirectory)
					? outputScreenshotsDirectory
					: $"{TestContext.TestRunResultsDirectory}\\Screenshots";

                driver.ScreenShotSaveFile(outputPath, testMethod);

				throw;
			}
		}

		[TestCleanup]
		public override void OnTestCleanup()
		{
			var testMethod = TestMethod;
			
			try
			{
                var webTestManager = ((WebTestManager)TestManager);

				CustomAttributesCleanup(testMethod);

				if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
				{
					TestContext.WriteLine(Infrastructure.Constants.Generic.TestErrorMessage);

					if (LoggingUtility == null || ConfigurationReader == null) return;

					var outputPath = !string.IsNullOrWhiteSpace(ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)) ? $"{ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)}" : $"{TestContext.TestRunResultsDirectory}{Constants.Configuration.ScreenshotsDirectory}";

					if (webTestManager.IsDriverNull(testMethod)) return;

					TestContext.WriteLine($"Attempting to capture screenshot to: {outputPath}");

                    webTestManager.WebTestMethodManager.WebDriverService.TryGetValue(testMethod, out IWebDriver driver);

					var captureScreenShot = driver.ScreenShotSaveFile(outputPath, testMethod);

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
				TestManager.OnTestCleanup(testMethod);
			}
		}

		//public IWebDriver Driver
		//{
		//	get
		//	{
		//		((WebTestManager) TestManager).WebTestMethodManager.WebDriverService.TryGetValue(TestMethod,
		//			out IWebDriver webDriver);
				
		//		return webDriver;
		//	}
		//}
		
		//public WebDriverWait WebDriverWait
		//{
		//	get
		//	{
		//		((WebTestManager) TestManager).WebTestMethodManager.WebDriverService.TryGetValue(TestMethod,
		//			out WebDriverWait webDriverWait);
				
		//		return webDriverWait;
		//	}
		//}

		public override HttpClient HttpClient => ((WebTestManager)TestManager).HttpClient;
    }
}




