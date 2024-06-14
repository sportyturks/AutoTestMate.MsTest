using System.Collections.Specialized;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class EmptyConfiguration : IConfiguration
    {
        public NameValueCollection Settings { get; set; } = [];
    }
}