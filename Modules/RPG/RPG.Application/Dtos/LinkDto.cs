namespace RPG.Application.Dtos
{
    public class LinkDto : Base.IDto<RPG.Domain.Entities.Link, LinkDto>
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Url { get; set; }
        public RPG.Domain.Entities.Link ToEntity() => DtoMapping.ToEntity(this);
        public static LinkDto ToDto(RPG.Domain.Entities.Link entity) => DtoMapping.ToDto(entity);
        public static explicit operator RPG.Domain.Entities.Link(LinkDto dto) => dto.ToEntity();
        public static explicit operator LinkDto(RPG.Domain.Entities.Link entity) => ToDto(entity);
    }
}
