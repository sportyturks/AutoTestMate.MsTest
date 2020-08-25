using AutoTestMate.MsTest.Infrastructure.Attributes;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Web.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.IntegrationTests
{
    [TestClass]
    public class ConfigurationReaderTests : WebTestBase
    { 
        [TestMethod]
        public void GetAppConfigConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("BrowserType") == "Chrome");
            Assert.IsTrue(configurationReader.GetConfigurationValue("ForceKillProcess") == "true");
            Assert.IsTrue(configurationReader.GetConfigurationValue("Headless") == "false");
            Assert.IsTrue(configurationReader.GetConfigurationValue("BrowserOs") == "Windows");
        }

        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void GetTestSettingConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }


        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void GetExcelConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void SetConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }


        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void UpdateConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void EnsureExceptionThrownForMandatoryConfigurationValues()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }

        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "3", SheetName = "TableOne")]
        public void ExcelTest1()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsNotNull(configurationReader);
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "3");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldOne") == "Over");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldTwo") == "The");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldThree") == "Tree");
        }


        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "5", SheetName = "TableTwo")]
        public void ExcelTest2()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsNotNull(configurationReader);
            Assert.IsTrue(configurationReader.GetConfigurationValue("BrowserType") == "Chrome");
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "5");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldFour") == "Sheep");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldFive") == "Have");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSix") == "You");
        }

        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void ExcelTest3()
        {
            var configurationReader = GetConfigurationReader();
            Assert.IsNotNull(configurationReader);
            Assert.IsTrue(configurationReader.GetConfigurationValue("BrowserType") == "Chrome");
            Assert.IsTrue(configurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(configurationReader.GetConfigurationValue("FieldNine") == "The");
        }
    }

}