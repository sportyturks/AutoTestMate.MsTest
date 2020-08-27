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
	        TestMethodManager.CheckTestAlreadyInitialised(testMethod);

	        try
            {
                TestMethodManager.Add(testMethod);
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
	            .Register(Component.For<ITestMethodManager>().ImplementedBy<TestMethodManager>().LifestyleSingleton())
	            .Register(Component.For<ITestManager>().Instance(this).OverridesExistingRegistration().LifeStyle.Singleton);

            if (TestContext.Properties["UseAppSettings"] != null && TestContext.Properties["UseAppSettings"].ToString().ToLower() == "false")//issue with linux
            {
                Container.Register(Component.For<IConfiguration>().ImplementedBy<EmptyConfiguration>().LifestyleSingleton());
            }
            else
            {
                Container.Register(Component.For<IConfiguration>().ImplementedBy<AppConfiguration>().LifestyleSingleton());
            }
        }
        
		public virtual void InitialiseIoc()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ILoggingUtility>().ImplementedBy<TestLogger>().LifestyleSingleton())
	            .Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>())
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
				TestMethodManager.UpdateConfigurationReader(testMethod, configurationReader);
			}
			else //TODO: check to see if this can be removed, possibly no longer required
			{
				var updateConfigurationReader = new ConfigurationReader(TestContext, AppConfiguration); 
				TestMethodManager.UpdateConfigurationReader(testMethod, updateConfigurationReader);
			}
		}
		public virtual void Dispose(string testMethod)
        {
            DisposeInternal(testMethod);
        }
        public virtual void DisposeInternal(string testMethod)
		{
			TestMethodManager.Dispose(testMethod);

            if (string.IsNullOrWhiteSpace(testMethod))
			{
				TestMethodManager.Dispose();

			}
		}

        public void SetTextContext(TestContext testContext)
        {
	        Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration());
        }

        #endregion
	}
}
