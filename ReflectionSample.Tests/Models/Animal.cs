namespace ReflectionSample.Tests.Models
{
    public class Animal
    {
        public string? Name { get; set; }

        private int? Age;

        public Animal() { }
        public Animal(string name, int? age = null)
        {
            Name = name;
            Age = age;
        }

        public void SetAge(int age)
        {
            Age = age;
        }
    }
}
