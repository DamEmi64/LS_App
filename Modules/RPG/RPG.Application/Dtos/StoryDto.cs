namespace RPG.Application.Dtos
{
    public class StoryDto : Base.IDto<RPG.Domain.Entities.Story, StoryDto>
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public List<ChapterDto> Chapters { get; set; } = new List<ChapterDto>();
        public Guid? Summary { get; set; }
        public List<RpgFileDto> Files { get; set; } = new();
        public RPG.Domain.Entities.Story ToEntity() => DtoMapping.ToEntity(this);
        public static StoryDto ToDto(RPG.Domain.Entities.Story entity) => DtoMapping.ToDto(entity);
        public static explicit operator RPG.Domain.Entities.Story(StoryDto dto) => dto.ToEntity();
        public static explicit operator StoryDto(RPG.Domain.Entities.Story entity) => ToDto(entity);

    }
}
