﻿using ReflectionSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ReflectionSample.Managers
{
    internal class ReadManager : BaseManager
    {
        internal static T GetModel<T>(string parsed, Type type)
        {
            var tokens = new List<Token>();
            var text = parsed.Trim();
            var length = text.Length;
            var cur = 0;

            if (text[cur] != '{')
            {
                throw new Exception("Не удалось распознать последовательность символов в начале");
            }

            while(cur < length - 1)
            {
                var token = GetNextToken(text, ref cur, length);
                tokens.Add(token);
                token.SetType();
            }

            //Проверка токенов на списки, классы, примитивы и т.д.
            //Пропускаю в первой итерации, рассматриваю только числа

            //Объект является классом - заполняем его поля и свойства
            if (type.IsClass)
            {
                return CreateInstanceClass<T>(type, tokens, 0);
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