using System;
using AutoTestMate.MsTest.Infrastructure.Constants;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Infrastructure.Data;
using AutoTestMate.MsTest.Infrastructure.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataTable = System.Data.DataTable;

namespace AutoTestMate.MsTest.Infrastructure.Attributes
{
    [AttributeUsage(validOn: AttributeTargets.Method, AllowMultiple = true)]
    public sealed class ExcelClosedTestDataAttribute : Attribute, ITestDataAttribute
    {
        #region Private Variables

        private string _sheetName; //i.e. Sheet1$
        private string _rowKey; //i.e. TC99999, used to get a single row of data
        private string _fileLocation; //i.e. TC99999, used to get a single row of data
        private string _fileName; //i.e. TC99999, used to get a single row of data

        #endregion

        #region Properties

        /// <summary>
        /// The excel sheet name, i.e. Sheet1$
        /// </summary>
        public string SheetName
        {
            get => !string.IsNullOrWhiteSpace(_sheetName) ? _sheetName : string.Empty;
            set => _sheetName = value;
        }

        /// <summary>
        /// Get the excel row with the key, i.e. TC99999, used to get a single row of data with the name TC99999
        /// </summary>
        public string RowKey
        {
            get => !string.IsNullOrWhiteSpace(_rowKey) ? _rowKey : string.Empty;
            set => _rowKey = value;
        }

        /// <summary>
        /// Get the excel row with the key, i.e. TC99999, used to get a single row of data with the name TC99999
        /// </summary>
        public string FileLocation
        {
            get => !string.IsNullOrWhiteSpace(_fileLocation) ? _fileLocation : FileHelper.GetCurrentExecutingDirectory();
            set => _fileLocation = value;
        }

        /// <summary>
        /// Get the excel row with the key, i.e. TC99999, used to get a single row of data with the name TC99999
        /// </summary>
        public string FileName
        {
            get => !string.IsNullOrWhiteSpace(_fileName) ? _fileName : string.Empty;
            set => _fileName = value;
        }

        #endregion

        #region Methods
        public void BeforeTest(TestContext testContext, ITestManager testManager)
        {
            Exception exp = null;

            try
            {
                testManager.SetTextContext(testContext);
                var configurationReader = testManager.ConfigurationReader;
                var dt = ReadFile();
                UpdateConfigurationReader(dt, configurationReader);
                testManager.UpdateConfigurationReader(configurationReader);
            }
            catch (Exception ex)
            {
                exp = ex;
            }

            if (exp != null)
            {
                throw exp;
            }
        }
        public void AfterTest(ITestManager testManager)
        {
            testManager.UpdateConfigurationReader(null);
        }

        private DataTable ReadFile()
        {
            var excelLoader = new XlsLoader(FileLocation, FileName, SheetName,1);
            return excelLoader.AsDataTable();
        }

        private void UpdateConfigurationReader(DataTable dt, IConfigurationReader configurationReader)
        {
            var columnKeyIndex = ExcelTestData.ExcelMissingKeyIndex;

            for (var k = 0; k < dt.Columns.Count; k++)
            {
                if (string.Equals(dt.Columns[k].ColumnName.Trim().ToLower(), Constants.ExcelTestData.RowKey.Trim().ToLower()))
                {
                    columnKeyIndex = k;
                    break;
                }
            }

            if (columnKeyIndex == ExcelTestData.ExcelMissingKeyIndex) return;

            for (var r = 0; r < dt.Rows.Count; r++)
            {
                if (!string.Equals(dt.Rows[r][columnKeyIndex].ToString().ToLower(), RowKey.ToLower())) continue;

                for (var c = 0; c < dt.Columns.Count; c++)
                {
                    configurationReader.AddSetting(dt.Columns[c].ColumnName, dt.Rows[r][c].ToString());
                }

                return;
            }
        }

        #endregion
    }
}