using System;
using System.Collections.Generic;
using System.Linq;

namespace ReflectionSample.Models
{
    public class Token
    {
        public string Name { get; }
        public string TextValue { get; }
        public TokenType TokenType { get; set; }
        public int Level { get; set; }
        public List<Token> Childs { get; }
        public object Value { get; private set; }

        public Token(string name, string value, TokenType tokenType, int level = 0)
        {
            Name = name;
            TextValue = value;
            TokenType = tokenType;
            Level = level;
        }

        internal void SetType()
        {
            var text = TextValue.Trim();
            if (text.Length == 0)
            {
                TokenType = TokenType.Nullable;
                return;
            }

            if (text.First() == '"' && text.Last() == '"')
            {
                TokenType = TokenType.String;
                Value = TextValue.Substring(1, TextValue.Length - 2);
                return;
            }

            if (text.First() == '[' && text.Last() == ']')
            {
                TokenType = TokenType.IEnumerable;
                return;
            }

            if (text.First() == '{' && text.Last() == '}')
            {
                TokenType = TokenType.Class;
                return;
            }

            if (bool.TryParse(TextValue, out var result))
            {
                TokenType = TokenType.Boolean;
                Value = result;
                return;
            }

            if (TextValue == "null")
            {
                TokenType = TokenType.Nullable;
                return;
            }

            if (TextValue.First() == '-' && TextValue.Length > 1 && TextValue.Substring(1).All(e => char.IsDigit(e)))
            {
                TokenType = TokenType.Number;
                Value = GetNubmer(TextValue);
                return;
            }

            if (TextValue.All(e => char.IsDigit(e)))
            {
                TokenType = TokenType.Number;
                Value = GetNubmer(TextValue);
                return;
            }

            throw new Exception($"Не удалось определить тип токена {Name}");
        }

        private object GetNubmer(string value)
        {
            if (int.TryParse(value, out var intResult))
            {
                return intResult;
            }

            if (double.TryParse(value, out var doubleResult))
            {
                return doubleResult;
            }

            if (decimal.TryParse(value, out var decimalResult))
            {
                return decimalResult;
            }

            if (float.TryParse(value, out var floatResult))
            {
                return floatResult;
            }

            throw new Exception("Неизвестный тип данных");
        }
    }
}
