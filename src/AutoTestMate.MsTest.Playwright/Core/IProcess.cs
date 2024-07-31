using System.Collections.Generic;

namespace AutoTestMate.MsTest.Playwright.Core
{
    public interface IProcess
    {
        IEnumerable<int> GetProcessesByName(string name);
        int GetProcessesById(int id);
        void Kill(int id);
    }
}