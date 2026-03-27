using Base;

namespace Files.Domain.Entities
{
    public class File : Entity
    {
        public required string Title { get; set; }
        public Guid? Image { get; set; }
        public string? Locaction { get; set; }
        public Guid? Content { get; set; }
        public int FileType { get; set; }
        public AdditionalData? AdditionalData { get; set; }
        public required List<SourceLink> Sources { get; set; }
    }
}