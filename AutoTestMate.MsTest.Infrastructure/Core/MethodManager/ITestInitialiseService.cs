using System.Collections.Concurrent;

namespace AutoTestMate.MsTest.Infrastructure.Core.MethodManager
{
    public interface ITestInitialiseService
    {
        ConcurrentDictionary<string, bool> IsInitialisedList { get; set; }
        void AddOrUpdate(string testMethod, bool isInitialised);
        void Initialise(string testMethod);
        void CheckTestAlreadyInitialised(string testMethod);
        bool TryGetValue(string testMethod, out bool isInitialised);
        void Dispose(string testMethod);
        void Dispose();
    }
}