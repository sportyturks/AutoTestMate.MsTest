﻿namespace AutoTestMate.MsTest.Playwright.Constants
{
	public class Configuration
	{
		#region Public Constants

		public const string ConfigKeyBaseUrl = "BaseUrl";
		public const string ConfigKeyBrowserType = "BrowserType";
		public const string ConfigKeyBrowserParameters = "BrowserParameters";
		public const string ConfigKeyBrowserLocation = "BrowserLocation";
		public const string ConfigKeyTimeUrl = "Timeout";
		public const string ConfigKeyOutputFileScreenshotsDirectory = "OutputFileScreenshotsDirectory";
		public const string ConfigKeyOutputFileDirectory = "OutputFileDirectory";
		public const string ScreenshotsDirectory = "\\Screenshots";

		public const string LogLevelKey = "LogLevel";
		public const string LogNameKey = "LogName";
		public const string BaseUrlKey = "BaseUrl";
		public const string TimeoutKey = "Timeout";
		public const string OutputPathKey = "OutputPath";
		public const string BrowserTypeKey = "BrowserType";
		public const string BrowserParametersKey = "BrowserParameters";
		public const string BrowserLocationKey = "BrowserLocation";
		public const string BrowserProfileKey = "BrowserProfile";
		public const string DriverServerLocationKey = "DriverServerLocation";
		public const string IeDriverServerLocationKey = "IEDriverServerLocation";
		public const string NavigateToBasePageKey = "NavigateToBasePage";
        public const string LoginWaitTimeKey = "LoginWaitTime";
        public const string ForceKillProcessKey = "ForceKillProcess";
		public const string HeadlessKey = "Headless";
		public const string EnableDetailedLogging = "EnableDetailedLogging";
        public const string BrowserOsKey = "BrowserOs";
        public const string BrowserHdResolutionKey = "UseBrowserHdResolution";
		public const string BrowserOsWindows = "Windows";
        public const string BrowserOsLinux = "Linux";

		public const string UseSeleniumGridKey = "UseSeleniumGrid";
		public const string SeleniumGridUrlKey = "SeleniumGridUrl";
		public const string DefaultSeleniumUrl = "http://localhost:4444/wd/hub";
		public const string EnableVNC = "EnableVNC";
		public const string EnableVideo = "EnableVideo";
		public const string EnableLog = "EnableLog";
		public const string ScreenResolution = "ScreenResolution";
		public const string UseAop = "UseAop";
		public const int DefaultTimeoutValue = 5;
		public const string BrowserVersionKey = "BrowserVersion";


		#endregion
	}
}