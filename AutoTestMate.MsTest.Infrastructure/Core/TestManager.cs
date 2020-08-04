using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public class TestManager : ITestManager, IDisposable
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
		public bool IsInitialised { get; set; }
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
            Dispose();
            Container.Dispose();
        }

        public virtual void OnTestMethodInitialise(string testMethod, TestContext testContext = null)
        {
	        TestMethodManager.CheckTestAlreadyInitialised(testMethod);

	        try
            {
                InitialiseTestContext(testContext);
                IsInitialised = true;
            }
            catch (Exception exp)
            {
                LoggingUtility.Error(exp.Message);
                Dispose();
                throw;
            }
        }

        public virtual void OnTestCleanup()
        {
            Dispose();
        }

        public virtual void InitialiseTestContext(TestContext testContext = null)
        {
            if (testContext == null) return;

            Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration());
			Container.Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>().OverridesExistingRegistration());
		}

		public virtual void InitialiseIoc()
        {
            var container = new WindsorContainer();

            container.Register(Component.For<ILoggingUtility>().ImplementedBy<TestLogger>().LifestyleSingleton())
                .Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>())
                .Register(Component.For<IConfiguration>().ImplementedBy<AppConfiguration>().LifestyleSingleton())
                .Register(Component.For<IMemoryCache>().ImplementedBy<MemoryCache>().LifestyleSingleton())
                .Register(Component.For<ITestManager>().Instance(this).OverridesExistingRegistration().LifeStyle.Singleton);

            Container = container;
        }
		public virtual void InitialiseTestContextDependencies()
        {

        }
	    public virtual void UpdateConfigurationReader(IConfigurationReader configurationReader)
		{
			if (configurationReader != null)
			{
				Container.Register(Component.For<IConfigurationReader>().Instance(configurationReader).OverridesExistingRegistration());
			}
			else //Ensure ConfigurationReader is resolved from existing container dependencies 
			{
				Container.Register(Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>().OverridesExistingRegistration());
			}
		}
		public virtual void Dispose()
        {
            DisposeInternal();
        }
        public virtual void DisposeInternal()
		{
			IsInitialised = false;
		}

        public void SetTextContext(TestContext testContext)
        {
	        Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration());
        }

        #endregion
	}
}
