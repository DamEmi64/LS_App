using Base;

namespace Files.Domain.Entities
{
    public class AdditionalData : Entity
    {
        public int? GameGenre { get; set; }
        public string? Subject { get; set; }
        public int? Year { get; set; }
        public int? Semester { get; set; }
    }
}