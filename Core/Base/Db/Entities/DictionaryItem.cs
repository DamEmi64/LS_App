namespace Base.Entities
{
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
}