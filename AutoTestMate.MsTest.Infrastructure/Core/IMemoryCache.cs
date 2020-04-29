using System;

namespace AutoTestMate.MsTest.Infrastructure.Core
{
	public interface IMemoryCache
	{
		bool Add(string key, object value);
		bool Add(string key, object value, DateTimeOffset absExpiration);
		void Delete(string key);
		object GetValue(string key);

        bool Contains(string key);
    }
}