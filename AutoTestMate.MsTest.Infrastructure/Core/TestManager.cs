using System;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public class TestManager : ITestManager
	{
		#region Private Variables

		private static TestManager _uniqueInstance;
		private static readonly object SyncLock = new Object();

		#endregion

		#region Constructor

		protected TestManager() { }

		#endregion

		#region Properties
		public static TestManager Instance()
		{
			// Lock entire body of method
			lock (SyncLock)
			{
				// ReSharper disable once ConvertIfStatementToNullCoalescingExpression
				if (_uniqueInstance == null)
				{
					_uniqueInstance = new TestManager();
				}

				return _uniqueInstance;
			}
		}
		public WindsorContainer Container { get; set; }
		public ILoggingUtility LoggingUtility => Container.Resolve<ILoggingUtility>();
		public IConfigurationReader ConfigurationReader => Container.Resolve<IConfigurationReader>();
        public IConfiguration AppConfiguration => Container.Resolve<IConfiguration>();
        public ITestMethodManager TestMethodManager => Container.Resolve<ITestMethodManager>(); 
        public TestContext TestContext => Container.Resolve<TestContext>();

		#endregion

		#region Public Methods
        
        public virtual void OnInitialiseAssemblyDependencies(TestContext testContext = null)
        {
            InitialiseIoc();
            InitialiseTestContext(testContext);
            InitialiseTestContextDependencies();
        }
        public virtual void OnDisposeAssemblyDependencies()
        {
            Dispose(null);
            Container.Dispose();
        }

        public virtual void OnTestMethodInitialise(string testMethod, TestContext testContext = null)
        {
	        TestMethodManager.TestInitialiseService.CheckTestAlreadyInitialised(testMethod);

	        try
            {
                InitialiseTestContext(testMethod, testContext);
                TestMethodManager.TestInitialiseService.Initialise(testMethod);
            }
            catch (Exception exp)
            {
                LoggingUtility.Error(exp.Message);
                Dispose(testMethod);
                throw;
            }
        }

        public virtual void OnTestCleanup(string testMethod)
        {
            Dispose(testMethod);
        }

        public virtual void InitialiseTestContext(TestContext testContext = null)
        {
            if (testContext == null) return;

            Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration())
	            .Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>().OverridesExistingRegistration())
	            .Register(Component.For<ITestInitialiseService>().ImplementedBy<TestInitialiseService>().LifestyleSingleton())
	            .Register(Component.For<IConfigurationService>().ImplementedBy<ConfigurationService>().LifestyleSingleton())
	            .Register(Component.For<ITestMethodManager>().ImplementedBy<TestMethodManager>().LifestyleSingleton())
	            .Register(Component.For<ITestManager>().Instance(this).OverridesExistingRegistration().LifeStyle.Singleton);
        }
        
        public virtual void InitialiseTestContext(string testMethod, TestContext testContext = null)
        {
	        if (testContext == null) return;
	        
	        TestMethodManager.ConfigurationService.AddOrUpdate(testMethod, ConfigurationReader);
        }

		public virtual void InitialiseIoc()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ILoggingUtility>().ImplementedBy<TestLogger>().LifestyleSingleton())
	            .Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>())
	            .Register(Component.For<IConfiguration>().ImplementedBy<AppConfiguration>().LifestyleSingleton())
	            .Register(Component.For<IMemoryCache>().ImplementedBy<MemoryCache>().LifestyleSingleton());

            Container = container;
        }
		public virtual void InitialiseTestContextDependencies()
        {

        }
	    public virtual void UpdateConfigurationReader(string testMethod, IConfigurationReader configurationReader)
		{
			if (configurationReader != null)
			{
				TestMethodManager.ConfigurationService.AddOrUpdate(testMethod, configurationReader);
			}
			else //Ensure ConfigurationReader is resolved from existing container dependencies 
			{
				var updateConfigurationReader = new ConfigurationReader(TestContext, AppConfiguration);
				TestMethodManager.ConfigurationService.AddOrUpdate(testMethod, updateConfigurationReader);
			}
		}
		public virtual void Dispose(string testMethod)
        {
            DisposeInternal(testMethod);
        }
        public virtual void DisposeInternal(string testMethod)
		{
			TestMethodManager.TestInitialiseService.Dispose(testMethod);
			TestMethodManager.ConfigurationService.Dispose(testMethod);

			if (string.IsNullOrWhiteSpace(testMethod))
			{
				TestMethodManager.TestInitialiseService.Dispose();
				TestMethodManager.ConfigurationService.Dispose();
			}
		}

        public void SetTextContext(TestContext testContext)
        {
	        Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration());
        }

        #endregion
	}
}
