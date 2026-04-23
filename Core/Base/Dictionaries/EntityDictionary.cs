using System.Reflection;

namespace Base.Entities
{

    /// <summary>
    ///     Dictionary class
    /// </summary>

    public class EntityDictionary
    {
        public static DictionaryItem Item(int id, string name, string? description = null)
            => new()
            {
                Id = Guid.NewGuid(),
                Key = id,
                Name = name,
                Description = description
            };

        public static List<DictionaryItem> GetDictionaries()
        {
            var result = new List<DictionaryItem>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in GetLoadableTypes(assembly))
                {
                    try
                    {
                        var dictAttr = type.GetCustomAttribute<DictionaryAttribute>();
                        if (dictAttr != null)
                        {
                            string className = !string.IsNullOrWhiteSpace(dictAttr.Name) ? dictAttr.Name : type.Name;
                            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
                            {
                                if (prop.GetMethod != null && prop.GetMethod.IsStatic)
                                {
                                    var val = prop.GetValue(null);

                                    if (val is DictionaryItem d)
                                    {
                                        d.Dictionary = className;

                                        result.Add(d);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception) { }
                }
            }

            return result;
        }

        private static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null)!;
            }
        }
    }
}