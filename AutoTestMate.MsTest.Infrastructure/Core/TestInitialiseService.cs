using System;
using System.Collections.Concurrent;
using DocumentFormat.OpenXml.Wordprocessing;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
    public class TestInitialiseService : ITestInitialiseService, IDisposable
    {
        public TestInitialiseService()
        {
            IsInitialisedList = new ConcurrentDictionary<string, bool>();
        }
        
        public ConcurrentDictionary<string, bool> IsInitialisedList { get; set; }

        public void AddOrUpdate(string testMethod, bool isInitialised)
        {
            IsInitialisedList.AddOrUpdate(testMethod, isInitialised, (key, oldValue) => isInitialised);
        }

        public void Initialise(string testMethod)
        {
            IsInitialisedList.AddOrUpdate(testMethod, true, (key, oldValue) => true);
        }

        public void CheckTestAlreadyInitialised(string testMethod)
        {
            IsInitialisedList.TryGetValue(testMethod, out var isInitialised);
            
            if (isInitialised) throw new ApplicationException($"Test Method in [{testMethod}], has already been initialised");
        }

        public bool TryGetValue(string testMethod, out bool isInitialised)
        {
            return IsInitialisedList.TryGetValue(testMethod, out isInitialised);
        }

        public void Dispose(string testMethod)
        {
            IsInitialisedList.TryRemove(testMethod, out _);
        }

        public void Dispose()
        {
            IsInitialisedList.Clear();
        }
    }
}