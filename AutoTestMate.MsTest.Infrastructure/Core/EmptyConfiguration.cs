using System.Collections.Specialized;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class EmptyConfiguration : IConfiguration
    {
        public EmptyConfiguration()
        {
            Settings = new NameValueCollection();
        }
        public NameValueCollection Settings { get; set; }
    }
}