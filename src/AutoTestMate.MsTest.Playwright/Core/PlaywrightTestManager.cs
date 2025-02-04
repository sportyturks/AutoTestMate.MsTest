using System;
using System.Diagnostics.CodeAnalysis;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Core.Attributes;
using AutoTestMate.MsTest.Playwright.Core.Browser;
using AutoTestMate.MsTest.Playwright.Core.MethodManager;
using AutoTestMate.MsTest.Services.Core;
using Castle.MicroKernel.Registration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Playwright.Core
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlaywrightTestManager : ServiceTestManager
	{
		private static readonly Lazy<PlaywrightTestManager> Singleton = new(() => new PlaywrightTestManager());
		public new static PlaywrightTestManager Instance => Singleton.Value;
		
		private PlaywrightTestManager() { }
		
		public PlaywrightTestMethodManager PlaywrightTestMethodManager => (PlaywrightTestMethodManager)Container.Resolve<ITestMethodManager>();

        public bool IsDriverNull(string testMethod)
        {
	        PlaywrightTestMethodManager.TestMethods.TryGetValue(testMethod, out ITestMethodBase testMethodBase);
			var webTestMethod = (PlaywrightTestMethod) testMethodBase;
			//var driverExists = webTestMethod?.WebDriver != null;

	        //if (!driverExists) return true;

	        //return webTestMethod.WebDriver == null;

	        return false;
        }

		public static bool IsManagerNull()
		{
			return Singleton == null;
		}

        public override void OnInitialiseAssemblyDependencies(TestContext testContext = null)
        {
            base.OnInitialiseAssemblyDependencies(testContext);
            
            Container.Register(Component.For<IPlaywrightDriver>().ImplementedBy<PlaywrightDriver>().LifestyleSingleton());
            Container.Register(Component.For<IFactory<IDriverCleanup>>().ImplementedBy<BrowserCleanupFactory>().LifestyleSingleton());
            Container.Register(Component.For<ITestMethodManager>().ImplementedBy<PlaywrightTestMethodManager>().OverridesExistingRegistration().LifestyleSingleton());
            
			var browserOs = ConfigurationReader.GetConfigurationValue(Configuration.BrowserOsKey).ToLower().Trim();
            if (string.Equals(browserOs, Configuration.BrowserOsLinux.ToLower()))
            {
                Container.Register(Component.For<IProcess>().ImplementedBy<LinuxOsProcess>().LifestyleSingleton());
            }
            else
            {
                Container.Register(Component.For<IProcess>().ImplementedBy<WinOsProcess>().LifestyleSingleton());
            }

			if (!string.Equals(ConfigurationReader.GetConfigurationValue(Configuration.UseAop).Trim().ToLower(), Infrastructure.Constants.Generic.TrueValue)) return;
			
			RetryInterceptorRegister.Initialize(Container);
			
			//TODO: Not working for some reason
			//Container.Register(Classes.FromAssemblyInDirectory(new AssemblyFilter(FileHelper.GetCurrentExecutingDirectory())).BasedOn(typeof(PlaywrightBasePage)).LifestyleTransient());
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
			try
			{
				if (!string.IsNullOrWhiteSpace(testMethod))
				{
					LoggingUtility.Info($"Dispose test method -> {testMethod}...",true);
					TestMethodManager.Dispose(testMethod);
					return;
				}
				
				LoggingUtility.Info("Dispose all..",true);
				PlaywrightTestMethodManager.Dispose();
			}
			catch (System.Exception e)
			{
				LoggingUtility.Error(e.Message);
			}
		} 
	}
}
