namespace AutoTestMate.MsTest.Infrastructure.Core
{
    /// <summary>
    ///     Interface for the Framework's Logging Utility
    /// </summary>
    public interface ILoggingUtility
    {
        void Info(string message, bool logTestContext = false);
        void Error(string message, bool logTestContext = false);
        void Warning(string message, bool logTestContext = false);
        void Debug(string message, bool logTestContext = false);
    }
}
