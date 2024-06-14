using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	/// <summary>
	///     Framework configuration reader.
	/// </summary>
	public class ConfigurationReader : IConfigurationReader
    {
		public ConfigurationReader(TestContext testContext, IConfiguration appConfiguration)
        {
	        Settings = new Dictionary<string, string>();
	        TestContext = testContext;
			AppConfiguration = appConfiguration;

            if (testContext != null)
            {
                SetTestContext(testContext);
            }
        }

	    public ConfigurationReader(TestContext testContext, IConfiguration appConfiguration, IDictionary<string, string> settings)
	    {
		    TestContext = testContext;
		    AppConfiguration = appConfiguration;
			Settings = settings;
	    }

		#region Public Methods

		/// <summary>
		///     Gets a value from the configuration file.
		/// </summary>
		public string GetConfigurationValue(string key, bool required = false)
        {
	        if (Settings.Count == 0 && AppConfiguration.Settings.Count == 0)
	        {
		        throw new KeyNotFoundException($"{key} was not found in the test parameters. Please make sure that the solution has an active .runsettings file and that the parameter is valid.");
	        }

			var testSettingsValue = Settings.TryGetValue(key, out var setting) ? setting : string.Empty;
			if (!string.IsNullOrWhiteSpace(testSettingsValue))
            {
                return testSettingsValue;
            }

	        var appSettingsDict = AppConfiguration.Settings.AllKeys.ToDictionary(k => k, k => AppConfiguration.Settings[k]);
			var appSettingsValue = appSettingsDict.TryGetValue(key, out var value) ? value : string.Empty;
			if (!string.IsNullOrWhiteSpace(appSettingsValue))
            {
                return string.Equals(appSettingsValue, Constants.Configuration.NullValue) ? null : appSettingsValue;
            }

            if (!required)
            {
                return string.Empty;
            }
			
			throw new KeyNotFoundException( $"{key} was not found in the test parameters. Please make sure that the solution has an active .runsettings file and that the parameter is valid.");
        }
        public void SetTestContext(TestContext testContext)
        {
            var keys = testContext.Properties.Keys;

            foreach (var key in keys)
            {
                var value = testContext.Properties[key.ToString()].ToString();
                if (!Settings.TryGetValue(key.ToString(), out _))
                {
                    Settings.Add(key.ToString(), value);
                }
                else
                {
                    Settings[key.ToString()] = value;
                }
            }

            //TODO: Can always pass the data row into this method
            //if (testContext..DataRow?.Table != null)
            //{
            //    var dataRowKeys = testContext.DataRow.Table.Columns;
            //    foreach (DataColumn key in dataRowKeys)
            //    {
            //        var value = testContext.DataRow[key.ColumnName].ToString();
            //        string existingValue;
            //        if (!_settings.TryGetValue(key.ColumnName, out existingValue))
            //        {
            //            _settings.Add(key.ColumnName, value);
            //        }
            //        else
            //        {
            //            _settings[key.ColumnName] = value;
            //        }
            //    }
            //}
        }
        public void AddSetting(string key, string value)
        {
            if (!Settings.ContainsKey(key))
            {
                Settings.Add(key, value);
            }
        }

        public bool UpdateSetting(string key, string value)
        {
            if (!Settings.ContainsKey(key)) return false;

            Settings[key] = value;

            return true;
        }
        #endregion

        #region Public Properties
        public string LogLevel => GetConfigurationValue(Constants.Configuration.LogLevelKey);
        public string LogName => GetConfigurationValue(Constants.Configuration.LogNameKey);
        public IDictionary<string, string> Settings { get; }

        public IConfiguration AppConfiguration { get; }

        public TestContext TestContext { get; }

		#endregion
	}
}
