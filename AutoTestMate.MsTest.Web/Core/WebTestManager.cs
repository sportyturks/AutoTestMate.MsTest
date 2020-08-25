using System;
using AutoTestMate.MsTest.Infrastructure.Attributes;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Constants;
using AutoTestMate.MsTest.Web.Core.Browser;
using AutoTestMate.MsTest.Web.Core.MethodManager;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core
{
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

		public IWebDriver Browser(string testMethod)
		{
			if (IsDriverNull(testMethod))
			{
				throw new NullReferenceException("Test method not correctly initialised");
			}

            WebTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
            var webTestMethod = (WebTestMethod)testMethodBase;

            return webTestMethod?.WebDriver;
		}

        public IConfigurationReader ConfigurationReaderTestMethod(string testMethod)
        {
            if (IsDriverNull(testMethod))
            {
                throw new NullReferenceException("Test method not correctly initialised");
            }

            return TestMethodManager.TryGetValue(testMethod).ConfigurationReader;
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
				InitialiseTestContext(testMethod, testContext);
                TestMethodManager.Add(testMethod);
				//WebTestMethodManager.TestInitialiseService.Initialise(testMethod);
				//WebTestMethodManager.WebDriverService.StartWebDriver(testMethod);
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
		
		#region Recent Changes With No Parallel Execution
		
		//public bool IsDriverNull => !Container.Kernel.HasComponent(typeof(IWebDriver)) || Container.Resolve<IWebDriver>() == null;
		/*public WebDriverWait BrowserWait
      		{
      			get
      			{
                      if (IsDriverNull)
                      {
                          throw new NullReferenceException(Exceptions.ExceptionMsgWebBrowserWaitInstanceNotInitialised);
                      }
      
      				return Container.Resolve<WebDriverWait>();
      			}
      			set => Container.Register(Component.For<WebDriverWait>().Instance(value).OverridesExistingRegistration().LifestyleSingleton());
      		}*/  
		
		/*public IWebDriver Browser
		{
			get
			{
				if (IsDriverNull)
                {
                    throw new NullReferenceException(Exceptions.ExceptionMsgWebBrowserWaitInstanceNotInitialised);
				}

				return  Container.Resolve<IWebDriver>();
			}
			private set => Container.Register(Component.For<IWebDriver>().Instance(value).OverridesExistingRegistration().LifestyleSingleton());
		}*/
		
		/*public void StartWebDriver()
		{
			var browserDriverCleanupFactory = Container.Resolve<IFactory<IDriverCleanup>>();
			var browserFactory = Container.Resolve<IFactory<IWebDriver>>();

			var driverCleanup = browserDriverCleanupFactory.Create();
			driverCleanup.Initialise();
			Container.Register(Component.For<IDriverCleanup>().Instance(driverCleanup).OverridesExistingRegistration().LifestyleSingleton());

			Browser = browserFactory.Create();
			//Browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromMinutes(string.IsNullOrWhiteSpace(UiConfigurationReader.LoginWaitTime) ? 1 : Convert.ToInt32(UiConfigurationReader.LoginWaitTime));

			var browserWait = new WebDriverWait(Browser, TimeSpan.FromMinutes(string.IsNullOrWhiteSpace(ConfigurationReader.GetConfigurationValue(Configuration.LoginWaitTimeKey)) ? 1 : Convert.ToInt32(ConfigurationReader.GetConfigurationValue(Configuration.LoginWaitTimeKey))));
			BrowserWait = browserWait;

			IsInitialised = true;
		}*/
				/*public override void DisposeInternal(string testMethod)
		{
			try
			{
				if (IsInitialised && !IsDriverNull)
				{
					Browser?.Quit();
				}
			}
			catch (System.Exception exp)
			{
				LoggingUtility.Error(exp.Message);
			}
			finally
			{
				if (IsInitialised)
				{
					Browser?.Dispose();
					var driverCleanup = Container.Resolve<IDriverCleanup>();
					driverCleanup?.Dispose();
				}

				IsInitialised = false;
			}
		}*/
		
		#endregion
	}
}
