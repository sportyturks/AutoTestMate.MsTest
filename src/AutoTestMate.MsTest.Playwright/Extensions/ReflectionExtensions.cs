using System;
using System.Reflection;

namespace AutoTestMate.MsTest.Playwright.Extensions;

public static class ReflectionExtensions
{
    public static Object GetPropValue(this Object obj, String name) {
        foreach (String part in name.Split('.')) {
            if (obj == null) { return null; }

            Type type = obj.GetType();
            PropertyInfo info = type.GetProperty(part) ?? type.GetProperty(part, BindingFlags.NonPublic | BindingFlags.Instance);
           
            if (info == null)
            {
                var fi = type.GetField(part, BindingFlags.NonPublic | BindingFlags.Instance);
                obj = fi.GetValue(obj);
            }
            else
            {
                obj = info.GetValue(obj, null);  
            }
        }
       
        return obj;
    }
		
    public static T GetPropValue<T>(this Object obj, String name) {
        Object retval = GetPropValue(obj, name);
        if (retval == null) { return default(T); }

        // throws InvalidCastException if types are incompatible
        return (T) retval;
    }
}