namespace AutoTestMate.MsTest.Playwright.Core
{
	public interface IDriverCleanup
	{
		void Initialise();
		void Dispose();

		IProcess Process { get; }
	}
}