using System.ComponentModel;

namespace AutoTestMate.MsTest.Web.Enums
{
	public enum BrowserTypes
	{
		[Description("firefox")]
		Firefox,
		[Description("iexplore")]
		InternetExplorer,
		[Description("chrome")]
		Chrome,
		[Description("edge-chromium")]
		EdgeChromium,
		[Description("microsoftwebdriver")]
		Edge,
		[Description("")]
		NotSet
	}
}
