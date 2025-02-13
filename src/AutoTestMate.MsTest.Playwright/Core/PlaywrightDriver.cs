using System;
using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Core.Browser;
using AutoTestMate.MsTest.Playwright.Enums;
using Microsoft.Playwright;

namespace AutoTestMate.MsTest.Playwright.Core;

public sealed class PlaywrightDriver(IConfigurationReader configurationReader) : IPlaywrightDriver
{
    private IPage _page;
    private IBrowser _browser;
    private IBrowserContext _browserContext;

    public IPage Page => _page;
    public IBrowser Browser => _browser!;
    public IBrowserContext BrowserContext => _browserContext;
    public IPlaywright Playwright { get; private set;}
    public IConfigurationReader ConfigurationReader { get; } = configurationReader;
    public async Task Dispose()
    {
	    if (_browser != null)
	    {
		    await _page.CloseAsync().ConfigureAwait(false);
		    await _browser.CloseAsync().ConfigureAwait(false);
	    }
    }
    public async Task<IPage> StartPlaywright()
    {
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);
        
        _browser = await CreateBrowser().ConfigureAwait(false);
        
        _page = await _browser.NewPageAsync().ConfigureAwait(false);
        
        return _page;
    }
    public async Task<IBrowser> CreateBrowser()
        {
	        IBrowser browser;
            var loginWaitTime = Convert.ToInt64(ConfigurationReader.GetConfigurationValue(Configuration.LoginWaitTimeKey));
            var browserTypeValue = ConfigurationReader.GetConfigurationValue(Configuration.BrowserTypeKey);
            var browserType = !string.IsNullOrWhiteSpace(browserTypeValue) ? BrowserTypeMapper.ConvertBrowserValue(browserTypeValue) : BrowserTypes.Chrome;

            switch (browserType)
            {
                case BrowserTypes.Firefox:
                    browser = await CreateFirefoxWebDriver(loginWaitTime);
	                break;
                case BrowserTypes.InternetExplorer:
                    browser = await CreateInternetExplorerWebDriver(loginWaitTime);
	                break;
                case BrowserTypes.Chrome:
					browser = await CreateChromeWebDriver(loginWaitTime);
	                break;
                case BrowserTypes.Edge:
                    browser = await CreateEdgeWebDriver(loginWaitTime);
	                break;
                case BrowserTypes.NotSet:
	                browser = await CreateInternetExplorerWebDriver(loginWaitTime);
					break;
                default:
	                browser =await CreateInternetExplorerWebDriver(loginWaitTime);
					break;
            }

            return browser;
        }

	    public async  Task<IBrowser> CreateEdgeWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }

	    public async Task<IBrowser> CreateChromeWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync().ConfigureAwait(false);

		    if (string.Equals(ConfigurationReader.GetConfigurationValue(Configuration.UseSeleniumGridKey).ToLower(),
			        Infrastructure.Constants.Generic.TrueValue))
		    {
			    var seleniumGridUrl =
				    ConfigurationReader.GetConfigurationValue(Configuration
					    .SeleniumGridUrlKey); // Change if Selenium Grid is remote

			    var screenResolution = "screenResolution=1920x1080x24";
			    
			    var headless = string.Equals(ConfigurationReader.GetConfigurationValue(Configuration.HeadlessKey).ToLower(),
				    Infrastructure.Constants.Generic.FalseValue) ? "headless=false" : string.Empty;
			    
			    seleniumGridUrl += $"?{screenResolution}&{headless}";
			    
			    var launchOptions = new BrowserTypeConnectOptions
			    {
				    ExposeNetwork = "*.automation.delivery", // Expose all network requests
				    Timeout = 30000, // Timeout for launching browser (ms)
				    SlowMo = 100, // Adds delay between actions (ms)
				    //Devtools = true, // Open DevTools automatically
			    };

			    var browser = await playwright.Chromium.ConnectAsync(seleniumGridUrl, launchOptions).ConfigureAwait(false);
			    var context = await browser.NewContextAsync(new BrowserNewContextOptions
			    {
				    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }, // Set screen size
				    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Custom-Agent", // Custom User-Agent
				    Locale = "en-US", // Set browser language
				    TimezoneId = "America/New_York", // Set timezone
				    IgnoreHTTPSErrors = true // Ignore SSL errors,
				    
			    }).ConfigureAwait(false);
			    
			    _browserContext = context;
			    			    
			    return browser;
		    }
		    else
		    {
			    var launchOptions = new BrowserTypeLaunchOptions
			    {
				    Headless = false, // Set to true for headless mode
				    ExecutablePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe", // Set Chrome path explicitly
				    Timeout = 30000, // Timeout for launching browser (ms)
				    SlowMo = 100, // Adds delay between actions (ms)
				    //Devtools = true, // Open DevTools automatically
				    IgnoreDefaultArgs = ["true"], // Use default Playwright args
				    //IgnoreHTTPSErrors = true, // Ignore HTTPS certificate errors
				    Proxy = new Proxy { Server = "https://proxyserver.com:8080" }, // Set Proxy
				    Args =
				    [
					    "--start-maximized", // Start browser maximized
					    "--no-sandbox", // Bypass sandbox security (needed in some environments)
					    "--disable-infobars", // Disable “Chrome is being controlled” message
					    "--disable-popup-blocking", // Allow pop-ups
					    "--disable-notifications", // Disable notifications
					    "--disable-extensions", // Disable extensions
					    "--incognito", // Start browser in Incognito mode
					    "--disable-dev-shm-usage", // Fix crashes in Docker/Linux
					    "--disable-gpu", // Disable GPU acceleration (useful for CI/CD)
					    "--enable-automation", // Enable automation-related flags
					    "--mute-audio" // Mute all audio
				    ]
			    };

			    var browser = await playwright.Chromium.LaunchAsync(launchOptions);
			    var context = await browser.NewContextAsync(new BrowserNewContextOptions
			    {
				    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }, // Set viewport size
				    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Custom-Agent", // Custom User-Agent
				    Locale = "en-US", // Set browser locale
				    TimezoneId = "America/New_York", // Set timezone
				    //Geolocation = new Geolocation { Latitude = 40.7128, Longitude = -74.0060 }, // Set geolocation (New York)
				    Permissions = ["geolocation"], // Grant permissions
				    HasTouch = false, // Disable touch support
				    ColorScheme = ColorScheme.Dark // Set browser color scheme to dark mode
			    }).ConfigureAwait(false);
			    
			    _browserContext = context;
			    return browser;
		    }
	    }
	    public async Task<IBrowser>  CreateInternetExplorerWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }

	    public async Task<IBrowser>  CreateFirefoxWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }
}