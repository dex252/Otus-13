using Newtonsoft.Json;
using Xunit;

namespace ReflectionSample.Tests.Tests
{
    public class JsonSample
    {
        [Fact]
        public void Json()
        {
            var a = new List<B>();

            var b1 = new B();
            b1.Strings1 = new List<string>() { "aaa", "bbb" };
            b1.CC = new List<C>();

            var b2 = new B();
            b2.Strings1 = new List<string>() { "ccc", "ddd" };
            b2.CC = new List<C>();

            var c1 = new C();
            c1.Strings2 = new List<string>() { "b1c11", "b1c12" };
            c1.Ints = new List<int>() { 1, 2 };
            b1.CC.Add(c1);

            var c2 = new C();
            c2.Strings2 = new List<string>() { "b1c21", "b1c22" };
            c2.Ints = new List<int>() { 1, 2 };
            b1.CC.Add(c2);

            var c3 = new C();
            c3.Strings2 = new List<string>() { "b1c31", "b1c32" };
            c3.Ints = new List<int>() { 1, 2 };
            b2.CC.Add(c3);

            var c4 = new C();
            c4.Strings2 = new List<string>() { "b1c41", "b1c42" };
            c4.Ints = new List<int>() { 1, 2 };
            b2.CC.Add(c4);

            a.Add(b1);
            a.Add(b2);

            var json = JsonConvert.SerializeObject(a);
            Assert.NotEmpty(json);
        }

        [Fact]
        public void JsonPrimitive()
        {
            var num = 1;
            var fNum = 2.5f;
            var str = "aaa";
            var str2 = "\"Шел летний теплый дождь, вокруг цвела сирень\"";
            var list = new List<string>() { "111", "222" };
            var listEmpty = new List<string>();
            var d = new D() { Name = "Cat" };

            var jsonNum = JsonConvert.SerializeObject(num);
            Assert.NotEmpty(jsonNum);

            var jsonFNum = JsonConvert.SerializeObject(fNum);
            Assert.NotEmpty(jsonFNum);

            var jsonStr = JsonConvert.SerializeObject(str);
            Assert.NotEmpty(jsonStr);

            var jsonStr2 = JsonConvert.SerializeObject(str2);
            Assert.NotEmpty(jsonStr2);

            var jsonList = JsonConvert.SerializeObject(list);
            Assert.NotEmpty(jsonList);

            var jsonListEmpty = JsonConvert.SerializeObject(listEmpty);
            Assert.NotEmpty(jsonListEmpty);

            var jsonClass = JsonConvert.SerializeObject(d);
            Assert.NotEmpty(jsonClass);
        }

        public class B
        {
            public List<string> Strings1 { get; set; }
            public List<C> CC { get; set; }
        }

        public class C
        {
            public List<string> Strings2 { get; set; }
            public List<int> Ints { get; set; }
        }

        public class D
        {
            public string Name { get; set; }
        }
    }
}