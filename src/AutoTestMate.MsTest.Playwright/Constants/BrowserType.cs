using System.Diagnostics.CodeAnalysis;

namespace AutoTestMate.MsTest.Playwright.Constants
{
    [ExcludeFromCodeCoverage]
    public class BrowserType
    {
        #region Constants

        public const string Chrome = "chrome";
        public const string InternetExplorer = "internetexplorer";
        public const string Firefox = "firefox";
        public const string PhantomJs = "phantomjs";
        public const string Edge = "edge";

        #endregion
    }
}
