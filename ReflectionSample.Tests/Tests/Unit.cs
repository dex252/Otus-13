using ReflectionSample.Tests.Models;
using Xunit;

namespace ReflectionSample.Tests.Tests
{
    public partial class Unit
    {
        [Theory]
        [MemberData(nameof(TestDataString))]
        public void TestSerializeStringData(string obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataString))]
        public void TestDeserializeStringData(string equal, string obj)
        {
            var actual = Reflection.Deserialize<string>(obj);
            Assert.Equal(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersInt))]
        public void TestSerializeNumberIntData(int obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersInt))]
        public void TestDeserializeNumberIntData(int equal, string obj)
        {
            var actual = Reflection.Deserialize<int>(obj);
            Assert.Equal(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersDecimal))]
        public void TestSerializeNumberDecimalData(decimal obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersDecimal))]
        public void TestDeserializeNumberDecimalData(decimal equal, string obj)
        {
            var actual = Reflection.Deserialize<decimal>(obj);
            Assert.Equal(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersFloat))]
        public void TestSerializeNumberFloatData(float obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataNumbersFloat))]
        public void TestDeserializeNumberFloatData(float equal, string obj)
        {
            var actual = Reflection.Deserialize<float>(obj);
            Assert.Equal(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataList))]
        public void SerializeTestListClass(ListClass obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        /// <summary>
        /// Тест не является валидным, т.к. поля приватные. Смотреть результат следует в ручную.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="equal"></param>
        [Theory]
        [Obsolete]
        [MemberData(nameof(TestDataF))]
        public void SerializeTestF(FPrivate obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        /// <summary>
        /// Тест не является валидным, т.к. поля приватные. Смотреть результат следует в ручную.
        /// </summary>
        /// <param name="equal"></param>
        /// <param name="value"></param>
        [Theory]
        [Obsolete]
        [MemberData(nameof(TestDataF))]
        public void DeserializeTestF(FPrivate equal, string value)
        {
            var actual = Reflection.Deserialize<FPrivate>(value);

            Assert.Equivalent(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataAnimal))]
        public void SerializeTestAnimal(Animal obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataAnimal))]
        public void DeserializeTestAnimal(Animal equal, string value)
        {
            var actual = Reflection.Deserialize<Animal>(value);
            Assert.Equivalent(actual, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataHard))]
        public void SerializeTestHard(HardClass obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Fact]
        public void SerializeTestSingleData()
        {
            var num = 1; 
            var numEqual = "1";
            var jsonNum = Reflection.Serialize(num);
            Assert.Equal(jsonNum, numEqual);

            var fNum = 2.5f;
            var fNumEqual = "2.5";
            var jsonFNum = Reflection.Serialize(fNum);
            Assert.Equal(jsonFNum, fNumEqual);

            var str = "aaa";
            var strEqual = "\"aaa\"";
            var jsonStr = Reflection.Serialize(str);
            Assert.Equal(jsonStr, strEqual);

            var list = new List<string>() { "111", "222" };
            var listEqual = "[\"111\",\"222\"]";
            var jsonList = Reflection.Serialize(list);
            Assert.Equal(jsonList, listEqual);

            var listEmpty = new List<string>();
            var listEmptyEqual = "[]";
            var jsonListEmpty = Reflection.Serialize(listEmpty);
            Assert.Equal(jsonListEmpty, listEmptyEqual);
        }
    }
}