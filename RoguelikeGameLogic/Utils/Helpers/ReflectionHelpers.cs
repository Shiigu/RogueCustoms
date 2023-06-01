using System.Reflection;

namespace RoguelikeGameEngine.Utils.Helpers
{
    public class ReflectionHelpers
    {

        public static List<Type> GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToList();
        }
    }
}
