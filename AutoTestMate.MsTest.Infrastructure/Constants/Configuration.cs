using System.Diagnostics.CodeAnalysis;

namespace AutoTestMate.MsTest.Infrastructure.Constants
{
    [ExcludeFromCodeCoverage]
    public class Configuration
    {
        public const string LogLevelKey = "LogLevel";
		public const string LogNameKey = "LogName";
        public const string NullValue = "NULL";
        public const string OutputFileDirectory = "OutputFileDirectory";
    }
}
