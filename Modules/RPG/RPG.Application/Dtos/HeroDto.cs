using RPG.Domain.Entities;

namespace RPG.Application.Dtos
{
    public class HeroDto : Base.IDto<RPG.Domain.Entities.Hero, HeroDto>
    {
        public Guid? Id { get; set; }
        public Guid? Chapter { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Description { get; set; }
        public string? Player { get; set; }
        public string? Image { get; set; }
        public Guid? ImageId { get; set; }
        public string? PlayerData { get; set; }
        public List<Skill>? Skills { get; set; } = new List<Skill>();
        public RPG.Domain.Entities.Hero ToEntity() => DtoMapping.ToEntity(this);
        public static HeroDto ToDto(RPG.Domain.Entities.Hero entity) => DtoMapping.ToDto(entity);
        public static explicit operator RPG.Domain.Entities.Hero(HeroDto dto) => dto.ToEntity();
        public static explicit operator HeroDto(RPG.Domain.Entities.Hero entity) => ToDto(entity);
    }
}
