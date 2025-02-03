using System;
using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Playwright.Constants;
using AutoTestMate.MsTest.Playwright.Core.Browser;
using AutoTestMate.MsTest.Playwright.Enums;
using Microsoft.Playwright;

namespace AutoTestMate.MsTest.Playwright.Core;

public class PlaywrightDriver : IPlaywrightDriver
{
    private readonly Task<IPage> _page;
    private IBrowser _browser;

    public PlaywrightDriver(IConfigurationReader configurationReader)
    {
        ConfigurationReader = configurationReader;
        //_page = Task.Run(InitializePlaywright);
    }

    public Task<IPage> Page => _page;
    
    public IBrowser Browser => _browser!;
    public IPlaywright Playwright { get; private set;}
    public IConfigurationReader ConfigurationReader { get; }

    public void Dispose()
    {
        _browser?.CloseAsync();
    }
    public virtual async Task<IPage> StartPlaywright()
    {
        //Playwright
        Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
        
        //Browser
        _browser = await CreateBrowser();
        
        return await _browser.NewPageAsync();
    }
    
     public async Task<IBrowser> CreateBrowser()
        {
	        IBrowser browser;
            var loginWaitTime = Convert.ToInt64(ConfigurationReader.GetConfigurationValue(Constants.Configuration.LoginWaitTimeKey));
            var browserTypeValue = ConfigurationReader.GetConfigurationValue(Constants.Configuration.BrowserTypeKey);
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

	    public virtual async  Task<IBrowser> CreateEdgeWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }

	    public virtual async Task<IBrowser> CreateChromeWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();


		    if (string.Equals(ConfigurationReader.GetConfigurationValue(Configuration.UseSeleniumGridKey).ToLower(),
			        Infrastructure.Constants.Generic.TrueValue))
		    {
			    var seleniumGridUrl =
				    ConfigurationReader.GetConfigurationValue(Configuration
					    .SeleniumGridUrlKey); // Change if Selenium Grid is remote

			    var launchOptions = new BrowserTypeConnectOverCDPOptions()
			    {
				    Timeout = 30000, // Wait for up to 60s for connection
				    SlowMo = 100, // Adds delay between actions for debugging
			    };

			    var browser = await playwright.Chromium.ConnectOverCDPAsync(seleniumGridUrl, launchOptions);

			    var context = await browser.NewContextAsync(new BrowserNewContextOptions
			    {
				    ViewportSize = new ViewportSize { Width = 1920, Height = 1080 }, // Set screen size
				    UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) Custom-Agent", // Custom User-Agent
				    Locale = "en-US", // Set browser language
				    TimezoneId = "America/New_York", // Set timezone
				    IgnoreHTTPSErrors = true // Ignore SSL errors
			    });
			    
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
				    Proxy = new Proxy { Server = "http://proxyserver.com:8080" }, // Set Proxy
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
				    Permissions = new[] { "geolocation" }, // Grant permissions
				    HasTouch = false, // Disable touch support
				    ColorScheme = ColorScheme.Dark // Set browser color scheme to dark mode
			    });

			    return browser;
		    }
	    }
	    

	    public virtual async Task<IBrowser>  CreateInternetExplorerWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }

	    public virtual async Task<IBrowser>  CreateFirefoxWebDriver(long loginWaitTime)
	    {
		    var playwright = await Microsoft.Playwright.Playwright.CreateAsync();
		    
		    var browser = await playwright.Firefox.LaunchAsync(new BrowserTypeLaunchOptions
		    {
			    Headless = false
		    });
		    
		    return browser;
	    }
}