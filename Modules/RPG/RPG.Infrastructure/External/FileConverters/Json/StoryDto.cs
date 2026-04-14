using RPG.Domain.Entities;

namespace RPG.Infrastructure.External.FileConverters.Json
{
    public class StoryDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public List<ChapterDto> Chapters { get; set; } = new List<ChapterDto>();

        public Guid? Summary { get; set; }
    }

    public class ChapterDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; } = 1;
        public List<HeroDto> Heroes { get; set; } = new List<HeroDto>();
        public List<PlaceDto> Places { get; set; } = new List<PlaceDto>();
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }

    public class HeroDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Description { get; set; }
        public string? Player { get; set; }
        public string? Image { get; set; }

        public PlayerData? PlayerData { get; set; }
    }

    public class PlaceDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }

        public string? Image { get; set; }
    }
}
