using System.Reflection;

namespace Base.Entities
{
    /// <summary>
    ///     Dictionaries saved in database
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class DictionaryAttribute : Attribute
    {
        public string Name { get; }

        public DictionaryAttribute(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    ///     Dictionary record item
    /// </summary>

    public class DictionaryItem
    {
        public Guid Id { get; set; }
        public required int Key { get; set; }
        public string Dictionary { get; set; } = "DEFAULT";
        public required string Name { get; set; }
        public string? Description { get; set; }

        public static implicit operator int(DictionaryItem d) => d.Key;
    }

    /// <summary>
    ///     Dictionary record
    /// </summary>

    public class EntityDictionary
    {
        public Guid Id { get; set; }
        public int Key { get; set; }
        public required string Dictionary { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }

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