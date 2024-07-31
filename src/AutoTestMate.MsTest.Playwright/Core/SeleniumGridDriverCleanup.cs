using AutoTestMate.MsTest.Infrastructure.Core;

namespace AutoTestMate.MsTest.Playwright.Core
{
	public class SeleniumGridDriverCleanupFactory(
		IConfigurationReader configurationReader,
#pragma warning disable CS9113 // Parameter is unread.
		ILoggingUtility loggingUtility,
#pragma warning restore CS9113 // Parameter is unread.
		IProcess osProcess)
		: IFactory<IDriverCleanup>
	{
		public virtual IDriverCleanup Create()
		{
			return new SeleniumGridDriverCleanup(osProcess, configurationReader);
		}
	}

#pragma warning disable CS9113 // Parameter is unread.
	public class SeleniumGridDriverCleanup(IProcess process, IConfigurationReader configurationReader) : IDriverCleanup
#pragma warning restore CS9113 // Parameter is unread.
	{
		public void Dispose()
		{
		}

		public void Initialise()
		{
		}

		public IProcess Process { get; } = process;
	}
}