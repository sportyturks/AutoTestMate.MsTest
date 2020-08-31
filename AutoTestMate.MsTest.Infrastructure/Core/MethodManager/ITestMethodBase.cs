namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public interface ITestMethodBase
    {
        string TestMethod { get; set; }
        bool IsInitialised { get; set; }
        IConfigurationReader ConfigurationReader { get; set; }
        ILoggingUtility LoggingUtility { get; set; }
    }
}