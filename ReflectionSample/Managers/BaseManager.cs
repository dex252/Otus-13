using System;
using System.Linq;
using System.Reflection;

namespace ReflectionSample.Managers
{
    internal class BaseManager
    {
        protected const char ARRAY_START_SEPARATOR = '[';
        protected const char ARRAY_END_SEPARATOR = ']';
        protected const char ARRAY_ITEM_START_SEPARATOR = '{';
        protected const char ARRAY_ITEM_END_SEPARATOR = '}';
        protected const string NULL_VALUE = "null";
        protected const char POINT = '.';
        protected const char TUPLE_SEPARATOR = ':';
        protected const char MAIN_SEPARATOR = ',';
        protected const string DECIMAL_TYPE = "Decimal";
        protected const string FLOAT_TYPE = "Single";
        protected const string INT_TYPE = "Int32";

        /// <summary>
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
