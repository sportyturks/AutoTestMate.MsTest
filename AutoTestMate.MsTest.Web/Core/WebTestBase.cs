﻿using System.Net.Http;
using System.Runtime.CompilerServices;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using AutoTestMate.MsTest.Web.Extensions;
using Castle.MicroKernel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Web.Core
{
	public abstract class WebTestBase : ServiceTestBase
	{
		[TestInitialize]
		public override void OnTestInitialise()
		{
			var testMethod = TestMethod;
            TestManager = WebTestManager.Instance;

			try
			{
                WebTestManager.OnTestMethodInitialise(testMethod, TestContext);
				CustomAttributesInitialise(testMethod);
			}
			catch (System.Exception ex)
            {
				if (LoggingUtility == null || TestManager.ConfigurationReader == null) throw;

				LoggingUtility.Error(Constants.Exceptions.ExceptionMsgSetupError + ex.Message);

                WebTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
                var webTestMethod = (WebTestMethod)testMethodBase;

				if (webTestMethod?.WebDriver == null) throw;
				
				var outputScreenshotsDirectory = TestManager.ConfigurationReader.GetConfigurationValue("OutputScreenshotsDirectory");

				var outputPath = !string.IsNullOrWhiteSpace(outputScreenshotsDirectory) ? outputScreenshotsDirectory : $"{TestContext.TestRunResultsDirectory}\\Screenshots";

                webTestMethod.WebDriver.ScreenShotSaveFile(outputPath, testMethod);
                
                throw;
			}
		}

		[TestCleanup]
		public override void OnTestCleanUp()
		{
			var testMethod = TestMethod;
            TestManager = WebTestManager.Instance;
			WebTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
            var webTestMethod = (WebTestMethod)testMethodBase;

			try
			{
				CustomAttributesCleanup(testMethod);

				if (TestContext.CurrentTestOutcome != UnitTestOutcome.Passed)
				{
					TestContext.WriteLine(Infrastructure.Constants.Generic.TestErrorMessage);

					if (LoggingUtility == null || TestManager.ConfigurationReader == null) return;

					var outputPath = !string.IsNullOrWhiteSpace(TestManager.ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)) ? $"{TestManager.ConfigurationReader.GetConfigurationValue(Constants.Configuration.ConfigKeyOutputFileScreenshotsDirectory)}" : $"{TestContext.TestRunResultsDirectory}{Constants.Configuration.ScreenshotsDirectory}";

					if (webTestMethod?.WebDriver == null) return;

					TestContext.WriteLine($"Attempting to capture screen shot to: {outputPath}");

                    var captureScreenShot = webTestMethod.WebDriver.ScreenShotSaveFile(outputPath, testMethod);

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
                WebTestManager.OnTestCleanup(testMethod);
			}
		}

        public IWebTestMethodManager WebTestMethodManager => ((WebTestManager) TestManager).WebTestMethodManager;

		public WebTestManager WebTestManager => (WebTestManager)TestManager;

		public override HttpClient HttpClient => ((WebTestManager)TestManager).HttpClient;
		public virtual T GetPage<T>([CallerMemberName] string testName = null)
		{
			return TestManager.Container.Resolve<T>(new Arguments { { "testName", testName } });
		}
	}
}




