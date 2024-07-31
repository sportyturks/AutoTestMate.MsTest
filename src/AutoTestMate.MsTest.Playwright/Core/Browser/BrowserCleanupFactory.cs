using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Extensions;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Enums;

namespace AutoTestMate.MsTest.Playwright.Core.Browser
{
    public class BrowserCleanupFactory : IFactory<IDriverCleanup>
    {
        private readonly IConfigurationReader _configurationReader;
	    private readonly ILoggingUtility _loggingUtility;
        private readonly IProcess _osProcess;
		public BrowserCleanupFactory(IConfigurationReader configurationReader, ILoggingUtility loggingUtility, IProcess osProcess)
        {
            _configurationReader = configurationReader;
	        _loggingUtility = loggingUtility;
            _osProcess = osProcess;
        }

        public virtual IDriverCleanup Create()
        {
            string browserServer;
            string browserProcess;

            var browserTypeValue = _configurationReader.GetConfigurationValue(Constants.Configuration.BrowserTypeKey);
            var browserType = !string.IsNullOrWhiteSpace(browserTypeValue) ? BrowserTypeMapper.ConvertBrowserValue(browserTypeValue) : BrowserTypes.InternetExplorer;
            
            switch (browserType)
            {
                case Enums.BrowserTypes.InternetExplorer:
                    browserProcess = Enums.BrowserTypes.InternetExplorer.GetDescription();
                    browserServer = BrowserDriverServer.InternetExplorer;
                    break;
                case Enums.BrowserTypes.Chrome:
                    browserProcess = Enums.BrowserTypes.Chrome.GetDescription();
                    browserServer = BrowserDriverServer.Chrome;
                    break;
                case Enums.BrowserTypes.Firefox:
                    browserProcess = Enums.BrowserTypes.Firefox.GetDescription();
                    browserServer = BrowserDriverServer.Firefox;
                    break;
                default:
                    browserProcess = Enums.BrowserTypes.InternetExplorer.GetDescription();
                    browserServer = BrowserDriverServer.InternetExplorer;
                    break;
            }

            return new DriverCleanup(browserServer, browserProcess, _osProcess, _configurationReader);
        }
    }
}