using System;
using System.Collections.Generic;
using System.Text;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Core.MethodManager;
using DocumentFormat.OpenXml.Presentation;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoTestMate.MsTest.Web.Core.MethodManager
{
    public class WebTestMethod : TestMethodBase, IWebTestMethod
    {
        public WebTestMethod (IFactory<IDriverCleanup> driverCleanup, ILoggingUtility loggingUtility, IConfigurationReader configurationReader, string testMethod) : base(loggingUtility, configurationReader, testMethod)
        {
            DriverCleanup = driverCleanup;
        }

        public IWebDriver  WebDriver { get; set; }
        public WebDriverWait WebDriverWait { get; set; }
        public IFactory<IDriverCleanup> DriverCleanup { get; set; }

        public override void Dispose()
        {
            try
            {
                var testWebDriverExists = WebDriver != null;

                if (testWebDriverExists)
                {
                    WebDriver?.Quit();
                }

                WebDriverWait = null;
                
            }
            catch (System.Exception exp)
            {
                LoggingUtility.Error(exp.Message);
            }
            finally
            {
                if (IsInitialised)
                {
                    var driverCleanup = DriverCleanup.Create();
                    driverCleanup.Dispose();
                }

                IsInitialised = false;
            }


        }
    }
}
