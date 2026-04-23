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
}