using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Helpers
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public abstract class SpecificationContext
    {
        [TestInitialize]
        public virtual void Init()
        {
            Given();
            When();
        }
        public abstract void Given();
        public abstract void When();
    }
}
