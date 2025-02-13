using System;
using System.Threading.Tasks;
using AutoTestMate.MsTest.Infrastructure.Core;
using Microsoft.Playwright;

namespace AutoTestMate.MsTest.Playwright.Core;

public interface IPlaywrightDriver 
{
    IPage Page { get; }
    IBrowser Browser { get; }
    IBrowserContext BrowserContext { get; }
    IConfigurationReader ConfigurationReader { get; }
    IPlaywright Playwright { get; }
    Task Dispose();
    Task<IPage> StartPlaywright();
    Task<IBrowser> CreateBrowser();
    Task<IBrowser> CreateEdgeWebDriver(long loginWaitTime);
    Task<IBrowser> CreateChromeWebDriver(long loginWaitTime);
    Task<IBrowser>  CreateInternetExplorerWebDriver(long loginWaitTime);
    Task<IBrowser>  CreateFirefoxWebDriver(long loginWaitTime);
}