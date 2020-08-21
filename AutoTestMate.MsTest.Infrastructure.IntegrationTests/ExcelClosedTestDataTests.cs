using AutoTestMate.MsTest.Infrastructure.Attributes;
using AutoTestMate.MsTest.Infrastructure.Core;
using AutoTestMate.MsTest.Services.Core;
using AutoTestMate.MsTest.Web.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoTestMate.MsTest.Infrastructure.IntegrationTests
{
    [TestClass]
    public class ExcelClosedTestDataTests : WebTestBase
    {
        [TestMethod]
        [ExcelClosedTestData(FileLocation = @"./Data", FileName = "NurseryRhymesBook.xlsx", RowKey = "8", SheetName = "TableThree")]
        public void EnsureCorrectFieldsAccessed()
        {
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("RowKey") == "8");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldSeven") == "Climbed");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldEight") == "Up");
            Assert.IsTrue(ConfigurationReader.GetConfigurationValue("FieldNine") == "The");
        }
    }
}