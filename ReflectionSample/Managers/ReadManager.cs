using System;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ReflectionSample.Managers
{
    internal class ReadManager : BaseManager
    {
        internal static object GetModel(string parsed, Type type)
        {
            if (type == typeof(string))
            {
                return ConvertToStr(parsed);
            }

            bool isNullablePrimitive = IsNullablePrimitive(type);
            if (type.IsPrimitive || isNullablePrimitive || type.Name == DECIMAL_TYPE)
            {
                return ConvertToNumber(parsed, type);
            }

            if (type.IsGenericType || type.IsArray)
            {
                throw new NotImplementedException();
            }

            if (type.IsClass)
            {
                return ConvertToClass(parsed, type);
            }

            throw new NotImplementedException();
        }

        private static object ConvertToClass(string parsed, Type type)
        {
            var instance = Activator.CreateInstance(type);

            string str = parsed;
            str = str.Trim();
            int firstPatternIndex = str.IndexOf('{');
            int lastPatternIndex = str.LastIndexOf('}');
            str = str.Remove(lastPatternIndex);
            str = str.Substring(firstPatternIndex + 1, str.Length - 1);

            if (string.IsNullOrWhiteSpace(str))
            {
                return instance;
            }

            var properties = type.GetProperties();
            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);

            foreach (var property in properties)
            {
                var propertyType = property.PropertyType;
                var propertyName = property.Name;

                var find = GetValueByObject(propertyType, propertyName, str);
                var model = GetModel(find, propertyType);
                SetValue(instance, propertyName, model);
            }

            foreach (var field in fields)
            {
                var isCompileGenerated = field.GetCustomAttributes<CompilerGeneratedAttribute>().Count() > 0;
                if (isCompileGenerated)
                {
                    continue;
                }

                var fieldType = field.FieldType;
                var fieldName = field.Name;

                var find = GetValueByObject(fieldType, fieldName, str);
                var model = GetModel(find, fieldType);
                SetValue(instance, fieldName, model);
            }

            return instance;
        }

        private static string GetValueByObject(Type type, string name, string parsed)
        {
            var str = parsed;
            int count = Regex.Matches(str, name).Count;
            if (count == 0)
            {
                throw new Exception("Не найдено вхождение строки в подстроку");
            }

            if (count > 1)
            {
                throw new NotImplementedException();
            }

            var indexStart = str.IndexOf(name);
            var cutter = str.Substring(indexStart + name.Length);
            var indexOfDelimiter = cutter.IndexOf(TUPLE_SEPARATOR);
            var cutterValue = cutter.Substring(indexOfDelimiter + 1);

            //Получаем содержание начала значения в cutterValue
            //В зависимости от начала содержимого - возвращаем блок его значений
            var value = cutterValue.TrimStart();
            if(value.First() == '{')
            {
                throw new NotImplementedException("Парсинг объекта внутри класса не реализован");
            }

            if (value.First() == '[')
            {
                throw new NotImplementedException("Парсинг массива внутри класса не реализован");
            }

            if (value.First() == '\"')
            {
                //Возвращаем содержимое строки, важно захватить экранированные символы
            }

            //Вероятно, содержимое - число, значит можно отдель значение до запятой или знака }
            var indexEndPaire = value.IndexOf(",");
            var indexEndObject = value.IndexOf("}");
            var indexEnd = -1;

            if(indexEndObject == -1 && indexEndPaire == -1)
            {
                indexEnd = value.Length;
            } 
            else if(indexEndObject == -1)
            {
                indexEnd = indexEndPaire;
            }
            else if(indexEndPaire == -1)
            {
                indexEnd = indexEndObject;
            }
            else
            {
                indexEnd = Math.Min(indexEndPaire, indexEndObject);
            }

            value = value.Substring(0, indexEnd);
            if (value == NULL_VALUE)
            {
                value = null;
            }

            return value;
        }

        private static object ConvertToStr(string parsed)
        {
            if (parsed == null)
            {
                return Convert.ChangeType(parsed, typeof(string));
            }

            string str = parsed.Trim();
            str = str.TrimStart('\"');
            int lastPatternIndex = str.LastIndexOf('\"');
            str = str.Remove(lastPatternIndex);
            str = str.Replace("\\\"", "\"");
            return Convert.ChangeType(str, typeof(string));
        }

        private static object ConvertToNumber(string parsed, Type type)
        {
            if (parsed == null)
            {
                return null;
            }
         
            var isNullable = IsNullablePrimitive(type);

            if (isNullable)
            {
                type = type.GetGenericArguments()[0];
            }

            var str = parsed?.Trim()?.Replace(".", ",");
            return Convert.ChangeType(str, type);
        }

    }
}
