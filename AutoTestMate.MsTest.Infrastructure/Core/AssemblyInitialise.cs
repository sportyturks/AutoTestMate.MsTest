using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	[TestClass]
	public static class AssemblyInitialise
	{
		[AssemblyInitialize]
		public static void Initialise(TestContext testContext)
		{
			TestManager.Instance.OnInitialiseAssemblyDependencies(testContext);
		}

		[AssemblyCleanup]
		public static void Cleanup()
		{
			TestManager.Instance.OnDisposeAssemblyDependencies();
		}
	}
}
