using System;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class TestManager : ITestManager
    {
        private static readonly Lazy<TestManager> Singleton = new(() => new TestManager());
        public static TestManager Instance => Singleton.Value;

        protected TestManager() { }

        public WindsorContainer Container { get; private set; }
        public ILoggingUtility LoggingUtility => Container.Resolve<ILoggingUtility>();
        public IConfigurationReader ConfigurationReader => Container.Resolve<IConfigurationReader>();
        public IConfiguration AppConfiguration => Container.Resolve<IConfiguration>();
        public ITestMethodManager TestMethodManager => Container.Resolve<ITestMethodManager>();
        public TestContext TestContext => Container.Resolve<TestContext>();

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

            Container.Register(
                Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration(),
                Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>().OverridesExistingRegistration(),
                Component.For<ITestMethodManager>().ImplementedBy<TestMethodManager>().LifestyleSingleton(),
                Component.For<ITestManager>().Instance(this).OverridesExistingRegistration().LifeStyle.Singleton
            );

            var useAppSettings = TestContext.Properties["UseAppSettings"]?.ToString().ToLower() != "false";
            var configurationType = useAppSettings ? typeof(AppConfiguration) : typeof(EmptyConfiguration);

            Container.Register(Component.For<IConfiguration>().ImplementedBy(configurationType).LifestyleSingleton());
        }

        public virtual void InitialiseIoc()
        {
            var container = new WindsorContainer(new DefaultKernel(new AutomationDependencyResolver(), null), new DefaultComponentInstaller());

            container.Register(
                Component.For<ILoggingUtility>().ImplementedBy<TestLogger>().LifestyleSingleton(),
                Component.For<IConfigurationReader>().ImplementedBy<ConfigurationReader>(),
                Component.For<IMemoryCache>().ImplementedBy<MemoryCache>().LifestyleSingleton()
            );

            Container = container;
        }

        public virtual void InitialiseTestContextDependencies()
        {
            // Implementation can be provided here or in inheritted classes 
        }

        public virtual void UpdateConfigurationReader(string testMethod, IConfigurationReader configurationReader)
        {
            var reader = configurationReader ?? new ConfigurationReader(TestContext, AppConfiguration);
            TestMethodManager.UpdateConfigurationReader(testMethod, reader);
        }

        public virtual void Dispose(string testMethod)
        {
            if (string.IsNullOrWhiteSpace(testMethod))
            {
                TestMethodManager.Dispose();
                return;
            }

            TestMethodManager.Dispose(testMethod);
        }

        public virtual void SetTestContext(TestContext testContext)
        {
            Container.Register(Component.For<TestContext>().Instance(testContext).OverridesExistingRegistration());
        }
    }
}