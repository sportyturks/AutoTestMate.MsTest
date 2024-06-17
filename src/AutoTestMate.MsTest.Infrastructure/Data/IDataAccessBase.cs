using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Infrastructure.Data;

public interface IDataAccessBase
{
    IConfigurationReader ConfigurationReader { get; }
    ILoggingUtility LoggingUtility { get; }
    string ConnectionString { get; }
}