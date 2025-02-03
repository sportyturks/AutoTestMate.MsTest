using System.Diagnostics.CodeAnalysis;

namespace AutoTestMate.MsTest.Playwright.Constants
{
    [ExcludeFromCodeCoverage]
    public class BrowserDriverServer
    {
        #region Constants

        public const string Chrome = "ChromeDriver";
        public const string InternetExplorer = "IEDriverServer";
        public const string Firefox = "geckodriver";
        public const string PhantomJs = "phantomjs";
        public const int DisposeTime = 2000;

        #endregion
    }
}
