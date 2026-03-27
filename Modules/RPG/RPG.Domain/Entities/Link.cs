using Base;

namespace RPG.Domain.Entities
{
    public class Link : Entity
    {
        public required string Title { get; set; }
        public required string Url { get; set; }
    }
}