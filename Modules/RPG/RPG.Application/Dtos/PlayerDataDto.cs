namespace RPG.Application.Dtos
{
    public class PlayerDataDto
    {
        public required string PlayerData { get; set; }
        public required List<SkillDto> Skills { get; set; }
    }

    public class SkillDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int? Value { get; set; }
        public Guid? SkillId { get; set; }
    }
}
