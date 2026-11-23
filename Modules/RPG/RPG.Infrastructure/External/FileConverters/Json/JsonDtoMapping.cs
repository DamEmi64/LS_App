using RPG.Domain.Entities;
namespace RPG.Infrastructure.External.FileConverters.Json
{
    internal static class JsonDtoMapping
    {
        internal static Story ToEntity(StoryDto dto) => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Summary = dto.Summary,
            Chapters = dto.Chapters.Select(c => ToEntity(c)).ToList()
        };
        internal static Chapter ToEntity(ChapterDto dto) => new()
        {
            Title = dto.Title,
            Description = dto.Description,
            Order = dto.Order,
            Heroes = dto.Heroes.Select(h => new Hero { FirstName = h.FirstName, LastName = h.LastName, Description = h.Description, Player = h.Player, PlayerData = h.PlayerData, Chapter = new Chapter { Id = Guid.Empty } }).ToList(),
            Places = dto.Places.Select(p => new Place { Title = p.Title, Description = p.Description, Chapter = new Chapter { Id = Guid.Empty } }).ToList(),
            Links = dto.Links.Select(kv => new Link { Title = kv.Key, Url = kv.Value }).ToList()
        };
        internal static StoryDto ToDto(Story entity) => new()
        {
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Summary = entity.Summary,
            Chapters = entity.Chapters.Select(c => ToDto(c)).ToList()
        };
        internal static ChapterDto ToDto(Chapter entity) => new()
        {
            Title = entity.Title,
            Description = entity.Description,
            Order = entity.Order,
            Heroes = entity.Heroes.Select(h => new HeroDto { FirstName = h.FirstName, LastName = h.LastName, Description = h.Description, Player = h.Player, PlayerData = h.PlayerData }).ToList(),
            Places = entity.Places.Select(p => new PlaceDto { Title = p.Title, Description = p.Description }).ToList(),
            Links = entity.Links.ToDictionary(l => l.Title, l => l.Url)
        };
    }
}
