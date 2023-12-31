﻿using ReflectionSample.Tests.Models;

namespace ReflectionSample.Tests.Tests
{
    public partial class Unit
    {
        private static FPrivate Exist = new FPrivate();

        public static object[][] TestDataF =>
          new[]
          {
                new object[] { new FPrivate(), "{\"i1\":0,\"i2\":0,\"i3\":0,\"i4\":0,\"i5\":0}" },
                new object[] { Exist.Get(), "{\"i1\":1,\"i2\":2,\"i3\":3,\"i4\":4,\"i5\":5}" }

          };

        public static object[][] TestDataAnimal =>
          new[]
          {
                new object[] { new Animal(), "{\"Name\":null,\"Age\":null}" },
                new object[] { new Animal() { Name = "Бобик" },  "{\"Name\":\"Бобик\",\"Age\":null}"},
                new object[] { new Animal("Мурзик"),  "{\"Name\":\"Мурзик\",\"Age\":null}"},
                new object[] { new Animal("Каркарыш", 12), "{\"Name\":\"Каркарыш\",\"Age\":12}" }

          };

        public static object[][] TestDataList =>
         new[]
         {
                new object[] { new ListClass(), "{\"Strings\":[],\"Ints\":[],\"Array\":[]}", },
                new object[] { new ListClass() { Array = new string[2] { "ccc", "ddd"} }, "{\"Strings\":[],\"Ints\":[],\"Array\":[\"ccc\",\"ddd\"]}", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"} }, "{\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[],\"Array\":[]}", },
                new object[] { new ListClass() { Ints = new List<int>() { 1, 2, 3} }, "{\"Strings\":[],\"Ints\":[1,2,3],\"Array\":[]}", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"}, Ints = new List<int>() { 1, 2, 3 } }, "{\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[1,2,3],\"Array\":[]}", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"}, Ints = new List<int>() { 1, 2, 3 }, Array = new string[2] { "ccc", "ddd" } }, "{\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[1,2,3],\"Array\":[\"ccc\",\"ddd\"]}", }

         };

        public static object[][] TestDataHard =>
         new[]
         {
                new object[] { new HardClass(), "{\"Name\":null,\"Simple\":{\"Name\":null,\"Age\":null},\"SimpleList\":[],\"Age\":5}" },
                new object[] { new HardClass() { SimpleList = new List<SimpleClass>()}, "{\"Name\":null,\"Simple\":{\"Name\":null,\"Age\":null},\"SimpleList\":[],\"Age\":5}" },
                new object[] { new HardClass() { Name = "Сложный класс", Simple = new SimpleClass() { Name = "Простой класс"} },  "{\"Name\":\"Сложный класс\",\"Simple\":{\"Name\":\"Простой класс\",\"Age\":3},\"SimpleList\":[],\"Age\":5}"},
                new object[] { new HardClass() { Name = "Сложный список", SimpleList = new List<SimpleClass>() { new SimpleClass() { Name = "Простой элемент списка 1"}, new SimpleClass() { Name = "Простой элемент списка 2" } } }, "{\"Name\":\"Сложный список\",\"Simple\":{\"Name\":null,\"Age\":null},\"SimpleList\":[{\"Name\":\"Простой элемент списка 1\",\"Age\":3},{\"Name\":\"Простой элемент списка 2\",\"Age\":3}],\"Age\":5}" },
         };

        public static object[][] TestDataString =>
      new[]
      {
                new object[] { "aaa", "\"aaa\""},
                new object[] { "Вася:Вася", "\"Вася:Вася\""},
                new object[] { "11", "\"11\""},
                new object[] { "55.5", "\"55.5\""},
                new object[] { "77,07", "\"77,07\""},
                new object[] { "\"Шел летний теплый дождь, вокруг цвела сирень\"", "\"\\\"Шел летний теплый дождь, вокруг цвела сирень\\\"\""},
                new object[] { "\"Шел летний теплый дождь, вокруг цвела сирень\"", "\"\\\"Шел летний теплый дождь, вокруг цвела сирень\\\"\""},
                new object[] { "\"Шел летний теплый дождь\", вокруг цвела сирень\"", "\"\\\"Шел летний теплый дождь\\\", вокруг цвела сирень\\\"\""},
                new object[] { "\"\"Шел летний теплый дождь\", \"вокруг цвела сирень\"", "\"\\\"\\\"Шел летний теплый дождь\\\", \\\"вокруг цвела сирень\\\"\""},
      };

        public static object[][] TestDataNumbersInt =>
      new[]
      {
                new object[] { 111, "111"},
                new object[] { 0, "0"}
      };

        public static object[][] TestDataNumbersDecimal =>
      new[]
      {
                new object[] { 22m, "22"},
                new object[] { 22.02m, "22.02"},
                new object[] { 0.0m, "0.0"}
      };
        public static object[][] TestDataNumbersFloat =>
      new[]
      {
            new object[] { 1f, "1"},
            new object[] { 33.033f, "33.033"},
            new object[] { 0f, "0"}
      };

    }

}
