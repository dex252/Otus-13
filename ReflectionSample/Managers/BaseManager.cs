using System;
using System.Linq;
using System.Reflection;

namespace ReflectionSample.Managers
{
    internal class BaseManager
    {        /// <summary>
             /// Примитив допускающий значение null
             /// </summary>
             /// <param name="type"></param>
             /// <returns></returns>
        protected static bool IsNullablePrimitive(Type type)
        {

            //Примитив, допускающий значение null
            return type.IsGenericType
                                   && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                                   && type.GetGenericArguments()[0].IsPrimitive;
        }

        protected static void SetValue<T>(T target, string name, object value)
        {
            if (target == null) return;
            var type = target.GetType();
            var property = type.GetProperties().FirstOrDefault(f => f.Name.Equals(name));
            if (property != null)
            {
                property.SetValue(target, value, null);
                return;
            }

            var field = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly).FirstOrDefault(f => f.Name.Equals(name));
            if (field != null)
            {
                field.SetValue(target, value);
            }
        }
    }
}
