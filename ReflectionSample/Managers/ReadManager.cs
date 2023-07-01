using ReflectionSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace ReflectionSample.Managers
{
    internal class ReadManager : BaseManager
    {
        internal static T GetModel<T>(string parsed, Type type)
        {
            if (type == typeof(string))
            {
                string str = parsed;
                str = str.TrimStart('\"');
                int lastPatternIndex = str.LastIndexOf('\"');
                str = str.Remove(lastPatternIndex);
                str = str.Replace("\\\"", "\"");
                return (T)Convert.ChangeType(str, typeof(string));
            }

            bool isNullablePrimitive = IsNullablePrimitive(type);
            if (type.IsPrimitive || isNullablePrimitive)
            {
              
            }

            if (type.IsGenericType || type.IsArray)
            {
               
            }

           // EnrichSerializedData(obj, type, serialized);

            if (type.IsClass)
            {
              
            }

          

            throw new NotImplementedException();
        }





























        private static T CreateInstanceClass<T>(Type type, List<Token> tokens, int level)
        {
            var target = Activator.CreateInstance<T>();
            SetProperties(type, target, tokens, level);
            SetFields(type, target, tokens, level);

            return target;
        }

        private static void SetProperties<T>(Type type, T target, List<Token> tokens, int level)
        {
            var properties = type.GetProperties();

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var propertyName = property.Name;

                SetToObject(propertyType, propertyName, target, tokens, level);
            }
        }

        private static void SetFields(Type type, object target, List<Token> tokens, int level)
        {
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

                SetToObject(fieldType, fieldName, target, tokens, level);
            }
        }

        private static void SetToObject<T>(Type type, string name, T target, List<Token> tokens, int level)
        {
            var token = tokens.FirstOrDefault(e => e.Name == name && e.Level == level);
            if (token == null)
            {
                throw new Exception($"Токен с именем {name} на уровне вложенности {level} не найден");
            }

            SetValue(target, name, token.Value);

            //bool isNullablePrimitive = IsNullablePrimitive(type);
            //if (type.IsPrimitive || isNullablePrimitive)
            //{
            //    SetPrimitiveValue(target, name, tokens, level);
            //}

            //if (type == typeof(string))
            //{

            //}
        }

        private static void SetPrimitiveValue<T>(T target, string name, List<Token> tokens, int level)
        {
            var token = tokens.FirstOrDefault(e => e.Name == name && e.Level == level);
            if (token == null)
            {
                throw new Exception($"Токен с именем {name} на уровне вложенности {level} не найден");
            }

            SetValue(target, name, token.Value);
        }

        private static Token GetNextToken(string text, ref int cur, int allLength)
        {
            var keyStart = -1;
            var keyEnd = -1;

            //Ищем первое вхождение ключа
            while (cur < allLength - 1)
            {
                if(keyStart != -1 && keyEnd != -1)
                {
                    var name = text.Substring(keyStart, keyEnd - keyStart + 1);
                    return new Token(name, GetValue(text, ref cur, allLength), TokenType.Undefined);
                }

                cur++;
                var next = text[cur];

                if (next == ' ' && keyStart == -1)
                {
                    continue;
                }

                if (next == '"' && keyStart == -1)
                {
                    keyStart = cur + 1;
                    continue;
                }

                if (next == '"' && keyEnd == -1)
                {
                    keyEnd = cur-1;
                }
            }

            return null;
        }

        private static string GetValue(string text, ref int cur, int allLength)
        {
            var valStart = -1;
            var valEnd = -1;
            var value  = string.Empty;

            while (cur < allLength)
            {
                if (valStart != -1 && valEnd != -1)
                {
                    value = text.Substring(valStart, valEnd - valStart + 1);
                    return value;
                }

                cur++;
                var next = text[cur];
                if ((next == ' ' || next == ':') && valStart == -1)
                {
                    continue;
                }

                if (valStart == -1)
                {
                    valStart = cur;
                    continue;
                }

                if (next == ',' || cur == allLength - 1)
                {
                    valEnd = cur-1;
                    continue;
                }
            }

            valEnd = cur - 1;
            value = text.Substring(valStart, valEnd - valStart + 1);
            return value;
        }
    }
}
