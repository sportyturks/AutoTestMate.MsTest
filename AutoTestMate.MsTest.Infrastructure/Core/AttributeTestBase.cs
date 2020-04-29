using AutoTestMate.MsTest.Infrastructure.Attributes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public abstract class AttributeTestBase : CustomTestAttributes
	{
		[TestInitialize]
		public virtual void TestInitialize(ITestManager testManager)
		{
			CustomAttributesInitialise();
		}

		[TestCleanup]
		public virtual void TestCleanup(ITestManager testManager)
		{
			CustomAttributesCleanup();
		}
	}
}
