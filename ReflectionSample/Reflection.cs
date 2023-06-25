using ReflectionSample.Serilalize;

namespace ReflectionSample
{
    public static class Reflection
    {
        public static string Serialize(object obj)
        {
            var type = obj.GetType();
            var name = string.Empty;
                
            return Write.GetMembers(obj, name, type);
        }

        public static T Deserialize<T>(string obj)
            where T : class
        {
            return null;
        }
    }
}
