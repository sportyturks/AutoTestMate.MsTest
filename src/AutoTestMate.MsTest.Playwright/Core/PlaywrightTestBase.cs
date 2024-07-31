using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Playwright.Core.MethodManager;
using AutoTestMate.MsTest.Services.Core;
using Castle.MicroKernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Playwright.Core
{
	public abstract class PlaywrightTestBase : ServiceTestBase
	{
		[TestInitialize]
		public override void OnTestInitialise()
		{
			var testMethod = TestMethod;
            TestManager = PlaywrightTestManager.Instance;

			try
			{
                PlaywrightTestManager.OnTestMethodInitialise(testMethod, TestContext);
				CustomAttributesInitialise(testMethod);
			}
			catch (System.Exception ex)
            {
				if (LoggingUtility == null || TestManager.ConfigurationReader == null) throw;

				LoggingUtility.Error(Constants.Exceptions.ExceptionMsgSetupError + ex.Message);

                PlaywrightTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
                
                var webTestMethod = (PlaywrightTestMethod)testMethodBase;

				//if (webTestMethod?.WebDriver == null) throw;

				CaptureScreenshot(testMethod);
                
                throw;
			}
		}

		[TestCleanup]
		public override void OnTestCleanUp()
		{
			var testMethod = TestMethod;
            TestManager = PlaywrightTestManager.Instance;
            
			try
			{
				CustomAttributesCleanup(testMethod);

				if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
				{
					TestContext.WriteLine(Infrastructure.Constants.Generic.TestErrorMessage);

					if (LoggingUtility == null || TestManager.ConfigurationReader == null) return;

					var screenshotPath = CaptureScreenshot(testMethod);

					LoggingUtility.Error(screenshotPath);
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
                PlaywrightTestManager.OnTestCleanup(testMethod);
			}
		}

        public IPlaywrightTestMethodManager PlaywrightTestMethodManager => ((PlaywrightTestManager) TestManager).PlaywrightTestMethodManager;

		public PlaywrightTestManager PlaywrightTestManager => (PlaywrightTestManager)TestManager;

		public override HttpClient HttpClient => ((PlaywrightTestManager)TestManager).HttpClient;
		public virtual T GetPage<T>([CallerMemberName] string testName = null)
		{
			return TestManager.Container.Resolve<T>(new Arguments { { "testName", testName } });
		}

		public virtual string CaptureScreenshot([CallerMemberName] string testMethod = null)
		{
			var screenshot = string.Empty;
			TestManager = PlaywrightTestManager.Instance;
			
			if (string.IsNullOrWhiteSpace(testMethod)) return screenshot; 
			
			PlaywrightTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
			var webTestMethod = (PlaywrightTestMethod)testMethodBase;
			
			var outputPath =
				!string.IsNullOrWhiteSpace(
					TestManager.ConfigurationReader.GetConfigurationValue(Constants.Configuration
						.ConfigKeyOutputFileScreenshotsDirectory))
					? $"{TestManager.ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)}"
					: $"{TestContext.TestRunResultsDirectory}{Constants.Configuration.ScreenshotsDirectory}";
					
			if (!string.IsNullOrWhiteSpace(outputPath) && outputPath.Contains("~")) //handle home relative paths
			{
				var homeDirRoot = Environment.GetEnvironmentVariable("HOME") + "/";
				var outputDir = outputPath.Trim('~').Replace("\\", "/");
				var homeDir = homeDirRoot + outputDir;
				outputPath = homeDir;
			}

			//if (webTestMethod?.WebDriver == null) return screenshot;
					
			TestContext.WriteLine($"Attempting to capture screen shot to: {outputPath}");

			//screenshot = webTestMethod.WebDriver.ScreenShotSaveFile(outputPath, testMethod);
			
			if (string.IsNullOrWhiteSpace(screenshot)) return string.Empty;

			TestContext.AddResultFile(screenshot);

			return screenshot;
		}  
	}
}




