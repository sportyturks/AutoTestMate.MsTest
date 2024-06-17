using System.Collections.Specialized;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public interface IConfiguration
    {
        NameValueCollection Settings { get; set; }
    }
}


