namespace AutoTestMate.MsTest.Playwright.Core.Attributes;

public interface  IRetryAttribute
{
    int Amount { get; set; }
    int Interval { get; set; }
}