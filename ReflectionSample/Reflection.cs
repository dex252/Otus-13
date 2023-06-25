using ReflectionSample.Serilalize;
using System;
using System.Reflection;

namespace ReflectionSample
{
    public static class Reflection
    {
        public static string Serialize<T>(T obj)
            where T : class
        {
            var type = typeof(T);
            var name = string.Empty;

            if (!type.IsGenericType)
            {
                name = type.Name;
            }
                
            return Write.GetMembers(obj, name, type);
        }

        public static T Deserialize<T>(string obj)
            where T : class
        {
            return null;
        }
    }
}
