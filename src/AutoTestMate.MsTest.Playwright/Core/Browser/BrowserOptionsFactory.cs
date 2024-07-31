using System;
using System.Collections.Generic;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Playwright.Enums;

namespace AutoTestMate.MsTest.Playwright.Core.Browser
{
    public class BrowserOptionsFactory : IFactory<DriverOptions>

    {

        protected readonly IConfigurationReader ConfigurationReader;

        protected readonly ILoggingUtility LoggingUtility;

        public BrowserOptionsFactory(IConfigurationReader configurationReader, ILoggingUtility loggingUtility)

        {

            ConfigurationReader = configurationReader;

            LoggingUtility = loggingUtility;

        }

        public virtual DriverOptions Create()

        {

            var browserTypeValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserTypeKey);

            var browserType = !string.IsNullOrWhiteSpace(browserTypeValue)
                ? BrowserTypeMapper.ConvertBrowserValue(browserTypeValue)
                : BrowserTypes.InternetExplorer;



            switch (browserType)

            {

                case BrowserTypes.Firefox:

                    return CreateFirefoxDriverOptions();

                case BrowserTypes.Chrome:

                    return CreateChromeDriverOptions();

                case BrowserTypes.EdgeChromium:

                    return CreateEdgeChromiumDriverOptions();

                case BrowserTypes.NotSet:

                    return CreateChromeDriverOptions();

                default:

                    return CreateChromeDriverOptions();

            }

        }



        protected virtual DriverOptions CreateFirefoxDriverOptions()

        {

            try

            {

                var options = new FirefoxOptions();

                var profileManager = new FirefoxProfileManager();

                var browserProfileSetting =
                    ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserProfileKey);

                var enableDetailLoggingSetting = ConfigurationReader
                    .GetConfigurationValue(Constants.Configuration.EnableDetailedLogging).ToLower();

                var headlessSetting = ConfigurationReader.GetConfigurationValue(Constants.Configuration.HeadlessKey)
                    .ToLower();

                LoggingUtility.Info(
                    $"Browser Profile: {browserProfileSetting}, Detailed Logging: {enableDetailLoggingSetting}, Headless: {headlessSetting}");



                var profile = profileManager.GetProfile(string.IsNullOrWhiteSpace(browserProfileSetting)
                    ? "default"
                    : browserProfileSetting);

                if (profile != null)

                {

                    options.AcceptInsecureCertificates = true;

                    //profile.AcceptUntrustedCertificates = true;

                    //profile.AssumeUntrustedCertificateIssuer = true;

                    options.Profile = profile;

                }



                options.AddArguments("--width-1920");

                options.AddArguments("--height-1080");



                if (string.Equals(enableDetailLoggingSetting, Infrastructure.Constants.Generic.TrueValue))

                {

                    options.LogLevel = FirefoxDriverLogLevel.Trace;

                }



                if (string.Equals(
                        ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableLog).ToLower(),
                        Infrastructure.Constants.Generic.TrueValue))

                {

                    var runName = "Test";

                    var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                    options.AddAdditionalFirefoxOption("logName", $"{runName}_{timestamp}.log");

                    options.AddAdditionalFirefoxOption("enableLog", true);

                }



                if (string.Equals(
                        ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVNC).ToLower(),
                        Infrastructure.Constants.Generic.TrueValue))

                {

                    options.AddAdditionalFirefoxOption("enableVNC", true);

                }



                if (string.Equals(
                        ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVideo).ToLower(),
                        Infrastructure.Constants.Generic.TrueValue))

                {

                    options.AddAdditionalFirefoxOption("enableVNC", true); // EnableVNC is overriden

                    var runName = "Test";

                    var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                    options.AddAdditionalFirefoxOption("videoName", $"{runName}_{timestamp}.mp4");

                    options.AddAdditionalFirefoxOption("enableVideo", true);

                }



                var screenResolution =
                    ConfigurationReader.GetConfigurationValue(Constants.Configuration.ScreenResolution);

                if (!string.IsNullOrEmpty(screenResolution))

                {

                    options.AddAdditionalFirefoxOption("screenResolution", screenResolution);

                }



                if (string.Equals(headlessSetting, Infrastructure.Constants.Generic.TrueValue))

                {

                    options.AddArguments("--headless");

                    /*options.AddArgument("disable-extensions");

                    options.AddArgument("disable-gpu");

                    options.AddArgument("disable-infobars");*/

                }



                SetBrowserVersion(options);



                return options;



            }

            catch (System.Exception e)

            {

                Console.WriteLine(e);

                LoggingUtility.Error($"Exeception: {e.Message}, Inner Exception: {e.InnerException?.Message}");

                throw;

            }

        }



        protected virtual DriverOptions CreateEdgeChromiumDriverOptions()

        {

            var options = new EdgeOptions

            {

                LeaveBrowserRunning = true

            };



            options.AddArgument("bwsi");



            //TODO add configuration check if this is should be done or not. For now we do it always

            foreach (var option in new[] { "ignore-certificate-errors", "allow-insecure-localhost" })

            {

                options.AddArgument(option);

                options.AddArgument($"--{option}");

            }



            //options.AddArgument("disable-extensions");

            options.AcceptInsecureCertificates = true;

            var browserHdResolution =
                ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserHdResolutionKey);

            if (browserHdResolution.ToLower().Equals("true"))

            {

                options.AddArgument("window-size=1920,1080");

            }

            options.AddArgument("start-maximized");

            options.AddArgument("allow-insecure-localhost");

            options.AddArgument("no-sandbox");

            options.AddAdditionalOption("useAutomationExtension", false);



            var browserProfile = ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserProfileKey);

            options.AddArgument(!string.IsNullOrWhiteSpace(browserProfile)

                ? $"profile-directory={browserProfile}"

                : "profile-directory=Profile 1");



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableLog).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                var runName = "Test";

                var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                options.AddAdditionalEdgeOption("logName", $"{runName}_{timestamp}.log");

                options.AddAdditionalEdgeOption("enableLog", true);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVNC).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddAdditionalOption("enableVNC", true);



                var moonOptions = new Dictionary<string, object> { { "enableVNC", true } };

                options.AddAdditionalOption("moon:options", moonOptions);



                var selenoidOptions = new Dictionary<string, object> { { "enableVNC", true } };

                options.AddAdditionalOption("selenoid:options", selenoidOptions);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVideo).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddAdditionalOption("enableVNC", true); // EnableVNC is overriden

                var runName = "Test";

                var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                options.AddAdditionalOption("videoName", $"{runName}_{timestamp}.mp4");

                options.AddAdditionalOption("enableVideo", true);

            }



            var screenResolution = ConfigurationReader.GetConfigurationValue(Constants.Configuration.ScreenResolution);

            if (!string.IsNullOrEmpty(screenResolution))

            {

                options.AddAdditionalOption("screenResolution", screenResolution);

            }



            if (string.Equals(
                    ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableDetailedLogging).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                var perfLogPrefs = new ChromiumPerformanceLoggingPreferences();

                perfLogPrefs.AddTracingCategories("devtools.network");

                options.PerformanceLoggingPreferences = perfLogPrefs;

                options.AddAdditionalEdgeOption(CapabilityType.EnableProfiling, true);

                options.SetLoggingPreference("performance", LogLevel.All);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.HeadlessKey).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddArguments("headless");

                options.AddArgument("verbose");

                options.AddArgument("disable-gpu");

                options.AddArgument("allow-running-insecure-content");

                options.AddAdditionalOption(CapabilityType.AcceptSslCertificates, true);

                options.AddAdditionalOption(CapabilityType.AcceptInsecureCertificates, true);

            }



            SetBrowserVersion(options);



            return options;

        }



        protected virtual DriverOptions CreateChromeDriverOptions()

        {

            var options = new ChromeOptions();



            options.LeaveBrowserRunning = true;

            options.AddArgument("bwsi");

            AddCertsIgnoreOptionsIfRequired(options);

            //options.AddArgument("disable-extensions");

            options.AcceptInsecureCertificates = true;

            var browserHdResolution =
                ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserHdResolutionKey);

            if (browserHdResolution.ToLower().Equals("true"))

            {

                options.AddArgument("window-size=1920,1080");

            }

            options.AddArgument("start-maximized");

            options.AddArgument("allow-insecure-localhost");

            options.AddArgument("no-sandbox");

            options.AddAdditionalOption("useAutomationExtension", false);



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableLog).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                var runName = "Test";

                var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                options.AddAdditionalChromeOption("logName", $"{runName}_{timestamp}.log");

                options.AddAdditionalChromeOption("enableLog", true);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVNC).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddAdditionalOption("enableVNC", true);



                var moonOptions = new Dictionary<string, object> { { "enableVNC", true } };

                options.AddAdditionalOption("moon:options", moonOptions);



                var selenoidOptions = new Dictionary<string, object> { { "enableVNC", true } };

                options.AddAdditionalOption("selenoid:options", selenoidOptions);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableVideo).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddAdditionalOption("enableVNC", true); // EnableVNC is overriden

                var runName = "Test";

                var timestamp = $"{DateTime.Now:yyyyMMdd_HHmm}";

                options.AddAdditionalOption("videoName", $"{runName}_{timestamp}.mp4");

                options.AddAdditionalOption("enableVideo", true);

            }



            var screenResolution = ConfigurationReader.GetConfigurationValue(Constants.Configuration.ScreenResolution);

            if (!string.IsNullOrEmpty(screenResolution))

            {

                options.AddAdditionalOption("screenResolution", screenResolution);

            }



            if (string.Equals(
                    ConfigurationReader.GetConfigurationValue(Constants.Configuration.EnableDetailedLogging).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                var perfLogPrefs = new ChromiumPerformanceLoggingPreferences();

                perfLogPrefs.AddTracingCategories("devtools.network");

                options.PerformanceLoggingPreferences = perfLogPrefs;

                options.AddAdditionalChromeOption(CapabilityType.EnableProfiling, true);

                options.SetLoggingPreference("performance", LogLevel.All);

            }



            if (string.Equals(ConfigurationReader.GetConfigurationValue(Constants.Configuration.HeadlessKey).ToLower(),
                    Infrastructure.Constants.Generic.TrueValue))

            {

                options.AddArguments("headless");

                options.AddArgument("verbose");

                options.AddArgument("disable-gpu");

                options.AddArgument("allow-running-insecure-content");

                options.AddAdditionalOption(CapabilityType.AcceptSslCertificates, true);

                options.AddAdditionalOption(CapabilityType.AcceptInsecureCertificates, true);

                /*options.AddArgument("disable-extensions");

                options.AddArgument("disable-gpu");

                options.AddArgument("disable-infobars");*/

            }



            SetBrowserVersion(options);



            return options;

        }



        private void AddCertsIgnoreOptionsIfRequired(ChromeOptions options)

        {

            //TODO add configuration check if this is should be done or not. For now we do it always

            foreach (var option in new[] { "ignore-certificate-errors", "allow-insecure-localhost" })

            {

                options.AddArgument(option);

                options.AddArgument($"--{option}");

            }



            //Not working in Selenium 4

            //options.AddAdditionalChromeOption(CapabilityType.AcceptInsecureCertificates, true);

        }



        private void SetBrowserVersion(DriverOptions options)

        {

            var browserVersion = ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserVersionKey);

            if (!string.IsNullOrWhiteSpace(browserVersion))

            {

                options.BrowserVersion = browserVersion;

            }



        }

    }
}