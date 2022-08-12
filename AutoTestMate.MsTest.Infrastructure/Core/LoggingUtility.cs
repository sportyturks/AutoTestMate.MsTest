using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class TestLogger : ILoggingUtility
    {
	    private readonly string _defaultFileName = $"{Environment.MachineName}-log.txt";
	    private readonly IConfigurationReader _configurationReader;
	    private readonly TestContext _testContext;

	    private ILogger _logger;
        public TestLogger(IConfigurationReader configurationReader, TestContext testContext)
        {
            _configurationReader = configurationReader;
            _testContext = testContext;
            SetupLogger();
        }

        [SuppressMessage("ReSharper", "StringLiteralTypo")]
        private void SetupLogger()
        {
            var config = new LoggingConfiguration();

            var consoleTarget = new ConsoleTarget();
            consoleTarget.Layout = @"${date:format=dd-MM-yyyy HH\:mm\:ss} [${level:uppercase=true}] Message -> ${message:withexception=true}";
            config.AddTarget("Console", consoleTarget);

            var fileTarget = new FileTarget();
            fileTarget.Layout = @"${date:format=dd-MM-yyyy HH\:mm\:ss} [${level:uppercase=true}] Message -> ${message:withexception=true}";
            config.AddTarget("File", fileTarget);

            var outputDirectory = _configurationReader.GetConfigurationValue(Constants.Configuration.OutputFileDirectory);
	        string outputFile;
	        
			if (!string.IsNullOrWhiteSpace(outputDirectory) && outputDirectory.Contains("/")) //handle relative paths
	        {
		        outputFile = $"{outputDirectory}/{_defaultFileName}";
	        }
			else if (!string.IsNullOrWhiteSpace(outputDirectory) && outputDirectory.Contains(@"\")) //handle absolute paths
	        {
		        // ReSharper disable once AssignNullToNotNullAttribute
		        outputFile = Path.Combine(Path.GetDirectoryName(outputDirectory), $"{_defaultFileName}");
			}
	        else //set default log file
	        {
		        outputFile = $"{FileHelper.GetCurrentExecutingDirectory()}/{_defaultFileName}";
	        }

			fileTarget.FileName = outputFile;
			
            // Step 4. Define rules
            var rule1 = new LoggingRule("*", LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

            _logger = LogManager.GetLogger("TestAutomationLogger");
        }

        /// <summary>
        ///     Logs message if log level is Info
        /// </summary>
        public void Info(string message,  bool logTestContext = false)
        {
            _logger.Info(message);
            TestContextWriteLine(message, logTestContext);
        }

        /// <summary>
        ///     Logs message if log level is Error
        /// </summary>
        public void Error(string message,  bool logTestContext = false)
        {
            _logger.Error(message);
            TestContextWriteLine(message, logTestContext);
        }

        /// <summary>
        ///     Logs message if log level is Warning
        /// </summary>
        public void Warning(string message,  bool logTestContext = false)
        {
            _logger.Warn(message);
            TestContextWriteLine(message, logTestContext);
        }

        /// <summary>
        ///     Logs message if log level is Debug
        /// </summary>
        public void Debug(string message,  bool logTestContext = false)
        {
            _logger.Debug(message);
            TestContextWriteLine(message, logTestContext);
        }

        private void TestContextWriteLine(string message, bool logTestContext)
        {
	        if (logTestContext)
	        {
		        _testContext.WriteLine(message);
	        }
        }
    }
}
