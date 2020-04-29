using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    /// <summary>
    ///     Interface for Framework Configuration Readers
    /// </summary>
    public interface IConfigurationReader
    {
	    void SetTestContext(TestContext testContext);
        void AddSetting(string key, string value);
        bool UpdateSetting(string key, string value);
        string GetConfigurationValue(string key, bool required = false);
        string LogLevel { get; }
        string LogName { get; }
	    IDictionary<string, string> Settings { get; }
	    IConfiguration AppConfiguration { get; }
		TestContext TestContext { get; }
	}
}