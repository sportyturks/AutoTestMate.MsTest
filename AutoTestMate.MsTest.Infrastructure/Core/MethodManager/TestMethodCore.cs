namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public class TestMethodCore(
        ILoggingUtility loggingUtility,
        IConfigurationReader configurationReader,
        string testMethod)
        : TestMethodBase(loggingUtility, configurationReader, testMethod);
}