namespace RPG.Application.Dtos
{
    public class ChapterDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Guid? Story { get; set; }
        public List<HeroDto> Heroes = new();
        public List<PlaceDto> Places = new();
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public int Order { get; set; }
        public List<SessionDto> Sessions { get; set; } = new List<SessionDto>();
        public List<LinkDto> Links { get; set; } = new List<LinkDto>();

        public bool Draft { get; set; }
    }
}