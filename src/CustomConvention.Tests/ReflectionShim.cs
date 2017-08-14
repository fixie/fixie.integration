using System;
using System.Reflection;

namespace CustomConvention.Tests
{
    public static class ReflectionShim
    {
        public static T GetCustomAttribute<T>(this Type type) where T : Attribute
            => type.GetTypeInfo().GetCustomAttribute<T>(inherit: true);
    }
}