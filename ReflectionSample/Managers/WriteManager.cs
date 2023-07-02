using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ReflectionSample.Managers
{
    internal class WriteManager: BaseManager
    {
        public static string GetMembers(object obj, string name, Type type)
        {
            var serialized = new List<string>();
            object value;

            if (type == typeof(string))
            {
                return ToStrStringType(obj, name, out value);
            }

            bool isNullablePrimitive = IsNullablePrimitive(type);
            if (type.IsPrimitive || isNullablePrimitive || type.Name == DECIMAL_TYPE)
            {
                return ToStrPrimitiveType(obj, name, out value);
            }

            if (type.IsGenericType || type.IsArray)
            {
                return ToStrGenericOrArrayType(obj, name, type, serialized);
            }

            EnrichSerializedData(obj, type, serialized);

            if (type.IsClass)
            {
                return ToStrClassType(name, serialized);
            }

            return string.Join(MAIN_SEPARATOR, serialized);
        }

        private static void EnrichSerializedData(object obj, Type type, List<string> serialized)
        {
            serialized.Add(GetProperties(obj, type));
            serialized.Add(GetFields(obj, type));
            serialized.RemoveAll(e => string.IsNullOrWhiteSpace(e));
        }

        private static string ToStrClassType(string name, List<string> serialized)
        {
            var members = string.Join(MAIN_SEPARATOR, serialized);
            if (!string.IsNullOrWhiteSpace(name))
            {
                return $"\"{name}\"{TUPLE_SEPARATOR}{ARRAY_ITEM_START_SEPARATOR}{members}{ARRAY_ITEM_END_SEPARATOR}";
            }

            return $"{ARRAY_ITEM_START_SEPARATOR}{members}{ARRAY_ITEM_END_SEPARATOR}";
        }

        private static string ToStrGenericOrArrayType(object obj, string name, Type type, List<string> serialized)
        {
            serialized.Add(GetGenerics(obj, type));
            var members = string.Join(MAIN_SEPARATOR, serialized);

            if (!string.IsNullOrWhiteSpace(name))
            {
                return $"\"{name}\"{TUPLE_SEPARATOR}{ARRAY_START_SEPARATOR}{members}{ARRAY_END_SEPARATOR}";
            }

            return $"{ARRAY_START_SEPARATOR}{members}{ARRAY_END_SEPARATOR}";
        }

        private static string ToStrPrimitiveType(object obj, string name, out object value)
        {
            value = obj == null ? NULL_VALUE : $"{obj}";

            if (!string.IsNullOrWhiteSpace(name))
            {
                return $"\"{name}\"{TUPLE_SEPARATOR}{value}".Replace(MAIN_SEPARATOR, POINT);
            }

            return $"{value}".Replace(MAIN_SEPARATOR, POINT);
        }

        private static string ToStrStringType(object obj, string name, out object value)
        {
            value = obj == null ? NULL_VALUE : $"\"{obj.ToString().Replace("\"","\\\"")}\"";
            if (!string.IsNullOrWhiteSpace(name))
            {
                return $"\"{name}\"{TUPLE_SEPARATOR}{value}";
            }

            return $"{value}";
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
                var value = obj == null ? null : field.GetValue(obj);

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

                var value = obj == null ? null : getter.Invoke(obj, null);

                var members = GetMembers(value, propertyName, propertyType);
                serialized.Add(members);
            }

            return string.Join(MAIN_SEPARATOR, serialized);
        }
    }
}
