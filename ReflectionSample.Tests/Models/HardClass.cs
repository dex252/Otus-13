namespace ReflectionSample.Tests.Models
{
    public class HardClass
    {
        public HardClass()
        {
            Age = 5;
        }

        public string? Name { get; set; }

        private int? Age;
        public SimpleClass? Simple { get; set; }

        public List<SimpleClass> SimpleList { get; set; }

    }

    public class SimpleClass
    {
        public string? Name { get; set; }
        private int? Age;

        public SimpleClass()
        {
            Age = 3;
        }

    }
}
