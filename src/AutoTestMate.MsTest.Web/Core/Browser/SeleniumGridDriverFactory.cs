using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Web.Constants;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
//using System.Text.Encoding.CodePages;

namespace AutoTestMate.MsTest.Web.Core.Browser
{
    [ExcludeFromCodeCoverage]
    public class SeleniumGridDriverFactory : IFactory<IWebDriver>
    {
        protected readonly IFactory<DriverOptions> BrowserOptionsFactory;
		public IConfigurationReader ConfigurationReader { get; set; }

        /// <summary>
        /// Used to create drivers based on the passed in runnsettings, appsetting or datarow stored in the configuration reader
        /// </summary>
        /// <param name="configurationReader">settings to be consumed while creating the driver</param>
        /// <param name="browserOptionsFactory"></param>
        public SeleniumGridDriverFactory(IConfigurationReader configurationReader, IFactory<DriverOptions> browserOptionsFactory)
        {
            ConfigurationReader = configurationReader;
            BrowserOptionsFactory = browserOptionsFactory;
        }

        public virtual IWebDriver Create()
        {
            var loginWaitTime = Convert.ToInt64(ConfigurationReader.GetConfigurationValue(Constants.Configuration.LoginWaitTimeKey));
			var cfgSeleniumGridUrl = ConfigurationReader.GetConfigurationValue(Constants.Configuration.SeleniumGridUrlKey);
			var seleniumGridUrl = string.IsNullOrEmpty(cfgSeleniumGridUrl) ? Configuration.DefaultSeleniumUrl : cfgSeleniumGridUrl;
            var driverOptions = BrowserOptionsFactory.Create();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //https://github.com/SeleniumHQ/selenium/issues/4816
            IWebDriver driver = new RemoteWebDriver(new Uri(seleniumGridUrl), driverOptions.ToCapabilities(), TimeSpan.FromMinutes(loginWaitTime));
            return driver;
        }
    }
}
