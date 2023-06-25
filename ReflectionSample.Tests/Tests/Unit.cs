using ReflectionSample.Tests.Models;
using Xunit;

namespace ReflectionSample.Tests.Tests
{
    public partial class Unit
    {

        [Theory]
        [MemberData(nameof(TestDataF))]
        public void SerializeTestF(F obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataAnimal))]
        public void SerializeTestAnimal(Animal obj, string equal)
        {
            var serialized = Reflection.Serialize(obj);
            Assert.Equal(serialized, equal);
        }

        [Theory]
        [MemberData(nameof(TestDataList))]
        public void SerializeTestListClass(ListClass obj, string equal)
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