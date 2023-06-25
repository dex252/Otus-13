using ReflectionSample.Tests.Models;

namespace ReflectionSample.Tests.Tests
{
    public partial class Unit
    {
        private static F Exist = new F();

        public static object[][] TestDataF =>
          new[]
          {
                new object[] { new F(), "\"i1\":0,\"i2\":0,\"i3\":0,\"i4\":0,\"i5\":0" },
                new object[] { Exist.Get(), "\"i1\":1,\"i2\":2,\"i3\":3,\"i4\":4,\"i5\":5" }

          };

        public static object[][] TestDataAnimal =>
          new[]
          { 
                new object[] { new Animal(), "\"Name\":null,\"Age\":null" },
                new object[] { new Animal() { Name = "Бобик" },  "\"Name\":\"Бобик\",\"Age\":null"},
                new object[] { new Animal("Мурзик"),  "\"Name\":\"Мурзик\",\"Age\":null"},
                new object[] { new Animal("Каркарыш", 12), "\"Name\":\"Каркарыш\",\"Age\":12" }

          };

        public static object[][] TestDataList =>
         new[]
         {      
                new object[] { new ListClass(), "\"Strings\":[],\"Ints\":[],\"Array\":[]", },
                new object[] { new ListClass() { Array = new string[2] { "ccc", "ddd"} }, "\"Strings\":[],\"Ints\":[],\"Array\":[\"ccc\",\"ddd\"]", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"} }, "\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[],\"Array\":[]", },
                new object[] { new ListClass() { Ints = new List<int>() { 1, 2, 3} }, "\"Strings\":[],\"Ints\":[1,2,3],\"Array\":[]", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"}, Ints = new List<int>() { 1, 2, 3 } }, "\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[1,2,3],\"Array\":[]", },
                new object[] { new ListClass() { Strings = new List<string>() { "aaa", "bbb", "ccc"}, Ints = new List<int>() { 1, 2, 3 }, Array = new string[2] { "ccc", "ddd" } }, "\"Strings\":[\"aaa\",\"bbb\",\"ccc\"],\"Ints\":[1,2,3],\"Array\":[\"ccc\",\"ddd\"]", }

         };
    }

}
