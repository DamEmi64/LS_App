using Newtonsoft.Json;
using RPG.Domain.Entities;
namespace RPG.Application.Dtos
{
    public static class DtoMapping
    {
        public static SessionDto ToDto(this Session x) => new() { Id = x.Id, Start = x.Start, End = x.End, Summary = x.Summary };
        public static Session ToEntity(this SessionDto x) => new() { Id = x.Id, Start = x.Start, End = x.End, Summary = x.Summary };
        public static LinkDto ToDto(this Link x) => new() { Id = Random.Shared.Next(), Title = x.Title, Url = x.Url };
        public static Link ToEntity(this LinkDto x) => new() { Title = x.Title, Url = x.Url };
        public static PlaceDto ToDto(this Place x) => new() { Id = x.Id, Title = x.Title, Description = x.Description, Chapter = x.Chapter?.Id, ImageId = x.Image };
        public static HeroDto ToDto(this Hero x) => new() { Id = x.Id, Chapter = x.Chapter?.Id, FirstName = x.FirstName, LastName = x.LastName, Description = x.Description, Player = x.Player, ImageId = x.Image, PlayerData = x.PlayerData?.Content, Skills = x.PlayerData?.Skills };
        public static Hero ToEntity(this HeroDto x) => new() { Id = x.Id ?? Guid.NewGuid(), FirstName = x.FirstName, LastName = x.LastName, Description = x.Description, Player = x.Player, Image = x.ImageId, Chapter = new Chapter { Id = x.Chapter ?? Guid.Empty }, PlayerData = (x.Skills?.Count > 0 || !string.IsNullOrWhiteSpace(x.PlayerData)) ? new PlayerData { Skills = x.Skills ?? new(), Content = x.PlayerData } : null };
        public static ChapterDto ToDto(this Chapter x, int depth = 0) => new() { Id = x.Id, Title = x.Title, Description = x.Description, Story = x.Story?.Id, Flow = JsonConvert.DeserializeObject<FlowDto>(x.FlowJson ?? string.Empty), StartDate = x.Sessions.Any() ? x.Sessions.Min(s => s.Start) : null, EndDate = x.Sessions.Any() ? x.Sessions.Max(s => s.End) : null, Heroes = depth >= 2 ? new() : x.Heroes.Select(h => h.ToDto()).ToList(), Places = depth >= 2 ? new() : x.Places.Select(p => p.ToDto()).ToList(), Sessions = x.Sessions.Select(s => s.ToDto()).ToList(), Links = x.Links.Select(l => l.ToDto()).ToList(), Order = x.Order, Draft = x.Draft };
        public static Chapter ToEntity(this ChapterDto x) => new() { Id = x.Id ?? Guid.NewGuid(), Title = x.Title, Description = x.Description, Order = x.Order, Draft = x.Draft, FlowJson = x.Flow is null ? null : JsonConvert.SerializeObject(x.Flow), Heroes = x.Heroes.Select(h => h.ToEntity()).ToList(), Places = x.Places.Select(p => p.ToEntity()).ToList(), Sessions = x.Sessions.Select(s => s.ToEntity()).ToList(), Links = x.Links.Select(l => l.ToEntity()).ToList(), Story = x.Story.HasValue ? new Story { Id = x.Story.Value } : null! };
        public static StoryDto ToDto(this Story x) => new() { Id = x.Id, Title = x.Title, Description = x.Description, StartDate = x.StartDate, EndDate = x.EndDate, Summary = x.Summary, Chapters = x.Chapters.Select(c => c.ToDto()).ToList(), Files = x.Files.Select(f => new RpgFileDto { Id = f.Id, Title = f.Title, Content = f.Content }).ToList() };
        public static Story ToEntity(this StoryDto x) => new() { Id = x.Id ?? Guid.NewGuid(), Title = x.Title, Description = x.Description, StartDate = x.StartDate, EndDate = x.EndDate, Summary = null, Chapters = x.Chapters.Select(c => c.ToEntity()).ToList(), Files = x.Files.Select(f => new RPGFile { Id = f.Id, Title = f.Title ?? string.Empty, Content = f.Content }).ToList() };
    }
}
