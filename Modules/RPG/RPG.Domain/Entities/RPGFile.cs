using Base;
namespace RPG.Domain.Entities
{
    public class RPGFile : Entity
    {
        public required string Title { get; set; }
        public required Guid Content { get; set; }
    }
}
