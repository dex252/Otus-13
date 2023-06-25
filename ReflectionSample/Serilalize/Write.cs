using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ReflectionSample.Serilalize
{
    internal class Write
    {
        const string ARRAY_START_SEPARATOR = "[";
        const string ARRAY_END_SEPARATOR = "]";
        const string ARRAY_ITEM_START_SEPARATOR = "{";
        const string ARRAY_ITEM_END_SEPARATOR = "}";
        const string NULL_VALUE = "null";
        const string POINT = ".";
        const string TUPLE_SEPARATOR = ":";
        const string MAIN_SEPARATOR = ",";

        public static string GetMembers(object obj, string name, Type type)
        {
            var serialized = new List<string>();
            object value;

            if (type == typeof(string))
            {
                value = obj == null ? NULL_VALUE : $"\"{obj}\"";
                if (!string.IsNullOrWhiteSpace(name))
                {
                    return $"\"{name}\"{TUPLE_SEPARATOR}{value}";
                }

                return $"{value}";
            }

            bool isNullablePrimitive = IsNullablePrimitive(type);
            if (type.IsPrimitive || isNullablePrimitive)
            {
                value = obj == null ? NULL_VALUE : $"{obj}";

                if (!string.IsNullOrWhiteSpace(name))
                {
                    return $"\"{name}\"{TUPLE_SEPARATOR}{value}".Replace(MAIN_SEPARATOR, POINT);
                }

                return $"{value}".Replace(MAIN_SEPARATOR, POINT);
            }

            if (type.IsGenericType || type.IsArray)
            {
                serialized.Add(GetGenerics(obj, type));
                var members = string.Join(MAIN_SEPARATOR, serialized);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    return $"\"{name}\"{TUPLE_SEPARATOR}{ARRAY_START_SEPARATOR}{members}{ARRAY_END_SEPARATOR}";
                }

                return $"{ARRAY_START_SEPARATOR}{members}{ARRAY_END_SEPARATOR}";
            }

            serialized.Add(GetProperties(obj, type));
            serialized.Add(GetFields(obj, type));
            serialized.RemoveAll(e => string.IsNullOrWhiteSpace(e));

            return string.Join(MAIN_SEPARATOR, serialized);
        }

        private static bool IsNullablePrimitive(Type type)
        {

            //Примитив, допускающий значение null
            return type.IsGenericType
                                   && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                                   && type.GetGenericArguments()[0].IsPrimitive;
        }

        private static string GetGenerics(object obj, Type type)
        {
            //Коллекция пустая
            if (obj == null)
            {
                return string.Empty;
            }

            var serialized = new List<string>();
            Type elemType = null;
            if (type.IsGenericType)
            {
                elemType = type.GetGenericArguments()[0];
            }
            else if (type.IsArray)
            {
                elemType = type.GetElementType();
            }
            else 
            {
                throw new NotImplementedException();
            }
            
            var isNullablePrimitive = IsNullablePrimitive(type);

            var collection = obj as IEnumerable;
            foreach (var item in collection)
            {
                //Пока не рассматриваю
                if (elemType.IsGenericType && !isNullablePrimitive)
                {
                    serialized.Add($"{ARRAY_ITEM_START_SEPARATOR}{GetMembers(item, null, elemType)}{ARRAY_ITEM_END_SEPARATOR}");
                    continue;
                }

                //Примитивы
                serialized.Add(GetMembers(item, null, elemType));
            }

            return string.Join(MAIN_SEPARATOR, serialized);
        }

        private static string GetFields(object obj, Type type)
        {
            var serialized = new List<string>();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
            
            foreach (var field in fields)
            {
                var isCompileGenerated = field.GetCustomAttributes<CompilerGeneratedAttribute>().Count() > 0;
                if (isCompileGenerated)
                {
                    continue;
                }
                
                var fieldType = field.FieldType;
                var fieldName = field.Name;
                var value = field.GetValue(obj);

                var members = GetMembers(value, fieldName, fieldType);
                serialized.Add(members);
            }

            return string.Join(MAIN_SEPARATOR, serialized);
        }

        private static string GetProperties(object obj, Type type)
        {
            var serialized = new List<string>();
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var getter = property.GetGetMethod();
                var propertyType = property.PropertyType;

                var propertyName = property.Name;
                var value = getter.Invoke(obj, null);

                var members = GetMembers(value, propertyName, propertyType);
                serialized.Add(members);
            }

            return string.Join(MAIN_SEPARATOR, serialized);
        }
    }
}
