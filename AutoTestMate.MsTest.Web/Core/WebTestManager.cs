using System;
using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Constants;
using AutoTestMate.MsTest.Web.Core.Browser;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;

namespace AutoTestMate.MsTest.Web.Core
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class WebTestManager : ServiceTestManager
	{
		#region Private Variables

		private static WebTestManager _uniqueInstance;
		private static readonly object SyncLock = new Object();

		#endregion

		#region Properties

		public new static WebTestManager Instance()
		{
			// Lock entire body of method
			lock (SyncLock)
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if (_uniqueInstance == null)
				{
					_uniqueInstance = new WebTestManager();
				}
				return _uniqueInstance;
			}
		}

        public static bool IsManagerNull()
		{
			lock (SyncLock)
			{
				return _uniqueInstance == null;
			}
		}
		
		public WebTestMethodManager WebTestMethodManager => (WebTestMethodManager)Container.Resolve<ITestMethodManager>();

        public bool IsDriverNull(string testMethod)
        {
	        WebTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
			var webTestMethod = (WebTestMethod) testMethodBase;
			var driverExists = webTestMethod?.WebDriver != null;

	        if (!driverExists) return false;

	        return webTestMethod.WebDriver == null;
        }

        #endregion

		#region Constructor

		private WebTestManager() { }

		#endregion

		#region Public Methods

        public override void OnInitialiseAssemblyDependencies(TestContext testContext = null)
        {
            base.OnInitialiseAssemblyDependencies(testContext);
            
            Container.Register(Component.For<IWebDriverService>().ImplementedBy<WebDriverService>().LifestyleSingleton());
            Container.Register(Component.For<ITestMethodManager>().ImplementedBy<WebTestMethodManager>().OverridesExistingRegistration().LifestyleSingleton());
            
			var browserOs = ConfigurationReader.GetConfigurationValue(Configuration.BrowserOsKey).ToLower().Trim();
            if (string.Equals(browserOs, Configuration.BrowserOsLinux.ToLower()))
            {
                Container.Register(Component.For<IProcess>().ImplementedBy<LinuxOsProcess>().LifestyleSingleton());
            }
            else
            {
                Container.Register(Component.For<IProcess>().ImplementedBy<WinOsProcess>().LifestyleSingleton());
            }

			if (string.Equals(ConfigurationReader.GetConfigurationValue(Configuration.UseSeleniumGridKey).ToLower(), Infrastructure.Constants.Generic.TrueValue))
			{
				Container.Register(Component.For<IFactory<IDriverCleanup>>().ImplementedBy<SeleniumGridDriverCleanupFactory>().LifestyleSingleton())
					.Register(Component.For<IFactory<DriverOptions>>().ImplementedBy<BrowserOptionsFactory>().LifestyleSingleton())
					.Register(Component.For<IFactory<IWebDriver>>().ImplementedBy<SeleniumGridDriverFactory>().LifestyleSingleton());
			}
			else
			{
				Container.Register(Component.For<IFactory<IDriverCleanup>>().ImplementedBy<BrowserCleanupFactory>().LifestyleSingleton())
					.Register(Component.For<IFactory<DriverOptions>>().ImplementedBy<BrowserOptionsFactory>().LifestyleSingleton())
					.Register(Component.For<IFactory<IWebDriver>>().ImplementedBy<BrowserFactory>().LifestyleSingleton());
			}
        }
        public override void OnTestMethodInitialise(string testMethod, TestContext testContext = null)
		{
			TestMethodManager.CheckTestAlreadyInitialised(testMethod);

			try
			{
                TestMethodManager.Add(testMethod);
			}
			catch (System.Exception exp)
			{
                LoggingUtility?.Error($"Exception Msg: {exp.Message}, Exception StackTrace: {exp.StackTrace}, Inner Exception Msg: {exp.InnerException?.Message} Inner Exception Stack Trace: {exp.InnerException?.StackTrace}");
				Dispose(testMethod);
				throw;
			}
		}

        public override void Dispose(string testMethod)
		{
			DisposeInternal(testMethod);
		}

		public override void DisposeInternal(string testMethod)
		{
			try
			{
				WebTestMethodManager.Dispose(testMethod);
            }
			catch (System.Exception e)
			{
				LoggingUtility.Error(e.Message);
			}
		}

		#endregion
    }
}
