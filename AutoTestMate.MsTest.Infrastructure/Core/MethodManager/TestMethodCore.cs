namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public class TestMethodCore : TestMethodBase
    {
        public TestMethodCore(ILoggingUtility loggingUtility, IConfigurationReader configurationReader, string testMethod): base(loggingUtility, configurationReader, testMethod)
        {
        }
    }
}