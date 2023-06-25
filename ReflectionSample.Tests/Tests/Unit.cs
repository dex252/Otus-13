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
    }
}