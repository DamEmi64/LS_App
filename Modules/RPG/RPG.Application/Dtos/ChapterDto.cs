namespace RPG.Application.Dtos
{
    public class ChapterDto : Base.IDto<RPG.Domain.Entities.Chapter, ChapterDto>
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

        public FlowDto? Flow { get; set; }
        public bool Draft { get; set; }
        public RPG.Domain.Entities.Chapter ToEntity() => DtoMapping.ToEntity(this);
        public static ChapterDto ToDto(RPG.Domain.Entities.Chapter entity) => DtoMapping.ToDto(entity);
        public static explicit operator RPG.Domain.Entities.Chapter(ChapterDto dto) => dto.ToEntity();
        public static explicit operator ChapterDto(RPG.Domain.Entities.Chapter entity) => ToDto(entity);
    }
}
