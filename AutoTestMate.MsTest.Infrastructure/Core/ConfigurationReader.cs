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
        #region Private Variables

        private readonly IDictionary<string, string> _settings;
        private readonly IConfiguration _appConfiguration;
	    private readonly TestContext _testContext;

		#endregion

		#region Constructor

		public ConfigurationReader(TestContext testContext, IConfiguration appConfiguration)
        {
	        _settings = new Dictionary<string, string>();
	        _testContext = testContext;
			_appConfiguration = appConfiguration;

            if (testContext != null)
            {
                SetTestContext(testContext);
            }
        }

	    public ConfigurationReader(TestContext testContext, IConfiguration appConfiguration, IDictionary<string, string> settings)
	    {
		    _testContext = testContext;
		    _appConfiguration = appConfiguration;
			_settings = settings;
	    }

		#endregion

		#region Public Methods

		/// <summary>
		///     Gets a value from the configuration file.
		/// </summary>
		public string GetConfigurationValue(string key, bool required = false)
        {
	        if (_settings.Count == 0 && _appConfiguration.Settings.Count == 0)
	        {
		        throw new KeyNotFoundException($"{key} was not found in the test parameters. Please make sure that the solution has an active .runsettings file and that the parameter is valid.");
	        }

			var testSettingsValue = _settings.ContainsKey(key) ? _settings[key] : string.Empty;
			if (!string.IsNullOrWhiteSpace(testSettingsValue))
            {
                return testSettingsValue;
            }

	        var appSettingsDict = _appConfiguration.Settings.AllKeys.ToDictionary(k => k, k => _appConfiguration.Settings[k]);
			var appSettingsValue = appSettingsDict.ContainsKey(key) ? appSettingsDict[key] : string.Empty;
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
                if (!_settings.TryGetValue(key.ToString(), out _))
                {
                    _settings.Add(key.ToString(), value);
                }
                else
                {
                    _settings[key.ToString()] = value;
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
            if (!_settings.ContainsKey(key))
            {
                _settings.Add(key, value);
            }
        }

        public bool UpdateSetting(string key, string value)
        {
            if (!_settings.ContainsKey(key)) return false;

            _settings[key] = value;

            return true;
        }
        #endregion

        #region Public Properties
        public string LogLevel => GetConfigurationValue(Constants.Configuration.LogLevelKey);
        public string LogName => GetConfigurationValue(Constants.Configuration.LogNameKey);
        public IDictionary<string, string> Settings => _settings;
		public IConfiguration AppConfiguration => _appConfiguration;
		public TestContext TestContext => _testContext;

		#endregion
	}
}
