using Base;

namespace Files.Domain.Entities
{
    public class SourceLink : Entity
    {
        public int SourceType { get; set; }

        public required string Link { get; set; }

        public bool Imported { get; set; }
    }
}