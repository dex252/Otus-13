using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace ReflectionSample.Serilalize
{
    internal class Write
    {
        const string ARRAY_START_SEPARATOR = "[";
        const string ARRAY_END_SEPARATOR = "]";
        const string ARRAY_ITEM_START_SEPARATOR = "{";
        const string ARRAY_ITEM_END_SEPARATOR = "}";
        const string NULL_VALUE = "null";

        public static string GetMembers(object obj, string name, Type type)
        {
            var serialized = new List<string>();
            object value;
            if (type == typeof(string))
            {
                if (obj == null)
                {
                    value = NULL_VALUE;
                }
                else
                {
                    value = $"\"{obj}\"";
                }

                return $"\"{name}\":{value}";
            }

            //Примитив, допускающий значение null
            bool isNullablePrimitive = type.IsGenericType
                                   && type.GetGenericTypeDefinition() == typeof(Nullable<>)
                                   && type.GetGenericArguments()[0].IsPrimitive;

            if (type.IsPrimitive || isNullablePrimitive)
            {
                if (obj == null)
                {
                    value = NULL_VALUE;
                }
                else
                {
                    value = $"{obj}";
                }
                return $"\"{name}\":{value}";
            }

            if (type.IsGenericType)
            {
                serialized.Add(GetGenerics(obj, type));
                var members = string.Join(",", serialized);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    return $"\"{name}\":{ARRAY_START_SEPARATOR}{members}{ARRAY_END_SEPARATOR}";
                }

                return members;
            }

            serialized.Add(GetProperties(obj, type));
            serialized.Add(GetFields(obj, type));
            serialized.RemoveAll(e => string.IsNullOrWhiteSpace(e));

            return string.Join(",", serialized);
        }

        private static string GetGenerics(object obj, Type type)
        {
            //Коллекция пустая
            if (obj == null)
            {
                return string.Empty;
            }

            var serialized = new List<string>();
            var elemType = type.GetGenericArguments()[0];

            var collection = obj as IEnumerable;
            foreach (var item in collection)
            {
                //Пока не рассматриваю
                if (elemType.IsGenericType)
                {
                    serialized.Add($"{ARRAY_ITEM_START_SEPARATOR}{GetMembers(item, null, elemType)}{ARRAY_ITEM_END_SEPARATOR}");
                    continue;
                }

                //Примитивы
                serialized.Add(GetMembers(item, null, elemType));
            }

            return string.Join(",", serialized);
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

            return string.Join(",",serialized);
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

            return string.Join(",", serialized);
        }
    }
}
