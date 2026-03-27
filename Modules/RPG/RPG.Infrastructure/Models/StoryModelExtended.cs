using RPG.Domain.Entities;

namespace RPG.Infrastructure.Models
{
    public class StoryModelExtended
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public List<ChapterModelExtended> Chapters { get; set; } = new List<ChapterModelExtended>();
        public byte[]? Summary { get; set; }
    }

    public class ChapterModelExtended
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Order { get; set; } = 1;
        public required List<HeroModelExtended> Heroes { get; set; }
        public required List<PlaceModelExtended> Places { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
        public List<Link> Links { get; set; } = new List<Link>();
    }

    public class HeroModelExtended
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Description { get; set; }
        public string? Player { get; set; }
        public string? Image { get; set; }
        public Guid? ImageId { get; set; }
        public PlayerData? PlayerData { get; set; }
    }

    public class PlaceModelExtended
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public string? Image { get; set; }
        public Guid? ImageId { get; set; }
    }
}