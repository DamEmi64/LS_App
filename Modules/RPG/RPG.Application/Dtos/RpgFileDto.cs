namespace RPG.Application.Dtos
{
    public class RpgFileDto : Base.IDto<RPG.Domain.Entities.RPGFile, RpgFileDto>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid Content { get; set; }
        public RPG.Domain.Entities.RPGFile ToEntity() => new() { Id = Id, Title = Title ?? string.Empty, Content = Content };
        public static RpgFileDto ToDto(RPG.Domain.Entities.RPGFile entity) => new() { Id = entity.Id, Title = entity.Title, Content = entity.Content };
        public static explicit operator RPG.Domain.Entities.RPGFile(RpgFileDto dto) => dto.ToEntity();
        public static explicit operator RpgFileDto(RPG.Domain.Entities.RPGFile entity) => ToDto(entity);
    }
}
