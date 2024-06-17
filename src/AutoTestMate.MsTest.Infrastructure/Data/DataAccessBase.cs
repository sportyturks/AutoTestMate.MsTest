using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Infrastructure.Data
{
    public class DataAccessBase(IConfigurationReader configReader, ILoggingUtility loggingUtility) : IDataAccessBase
    {
        public IConfigurationReader ConfigurationReader { get; } = configReader;
        public ILoggingUtility LoggingUtility { get; } = loggingUtility;
        public string ConnectionString => ConfigurationReader.GetConfigurationValue("AutomatedTestingDatabaseConnectionString");
    }
}