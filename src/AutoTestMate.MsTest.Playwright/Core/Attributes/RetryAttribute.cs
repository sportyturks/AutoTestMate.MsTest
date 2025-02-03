using System;

namespace AutoTestMate.MsTest.Playwright.Core.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class RetryAttribute : Attribute, IRetryAttribute
{
    public int Amount { get; set; }
    public int Interval { get; set; }
}