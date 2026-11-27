namespace RPG.Application.Dtos
{
    public class SessionDto : Base.IDto<RPG.Domain.Entities.Session, SessionDto>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public string? Summary { get; set; }
        public RPG.Domain.Entities.Session ToEntity() => DtoMapping.ToEntity(this);
        public static SessionDto ToDto(RPG.Domain.Entities.Session entity) => DtoMapping.ToDto(entity);
        public static explicit operator RPG.Domain.Entities.Session(SessionDto dto) => dto.ToEntity();
        public static explicit operator SessionDto(RPG.Domain.Entities.Session entity) => ToDto(entity);
    }
}
