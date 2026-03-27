namespace RPG.Infrastructure.Models
{
    public class SummaryModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required List<Guid> Chapters { get; set; }
        public bool IsPdf { get; set; }

        public bool All { get; set; }
    }
}