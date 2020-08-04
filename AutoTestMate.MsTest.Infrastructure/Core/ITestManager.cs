using Castle.Windsor;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public interface ITestManager
	{
        WindsorContainer Container { get; }
		TestContext TestContext { get; }
		IConfigurationReader ConfigurationReader { get; }
        IConfiguration AppConfiguration { get; }
		ILoggingUtility LoggingUtility { get; }
		void OnInitialiseAssemblyDependencies(TestContext testContext = null);
		void OnDisposeAssemblyDependencies();
        void OnTestMethodInitialise(string testMethod, TestContext testContext = null);
		void OnTestCleanup();
		void InitialiseIoc();
		void InitialiseTestContext(TestContext testContext = null);
		void InitialiseTestContextDependencies();
        void Dispose();
        void DisposeInternal();
		void UpdateConfigurationReader(IConfigurationReader configurationReader);
		void SetTextContext(TestContext testContext);
	}
}