using ReflectionSample.Managers;

namespace ReflectionSample
{
    public class Reflection
    {
        public static string Serialize(object obj)
        {
            var type = obj.GetType();
            var name = string.Empty;
                
            return WriteManager.GetMembers(obj, name, type);
        }

        public static T Deserialize<T>(string text)
        {
            var type = typeof(T);

            return (T)ReadManager.GetModel(text, type);
        }
    }
}
