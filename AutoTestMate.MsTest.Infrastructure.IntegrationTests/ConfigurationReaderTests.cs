using AutoTestMate.MsTest.Infrastructure.Attributes;
using AutoTestMate.MsTest.Infrastructure.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.IntegrationTests
{
    [TestClass]
    public class ConfigurationReaderTests : TestBase
    { 
        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void GetAppConfigConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void GetTestSettingConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }


        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void GetExcelConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void SetConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }


        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void UpdateConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelOdbcTestData(FileLocation = @".\Data", FileName = "NurseryRhymesBook.xls", RowKey = "8", SheetName = "TableThree")]
        public void EnsureExceptionThrownForMandatoryConfigurationValues()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }
    }

}