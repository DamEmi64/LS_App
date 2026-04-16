using Base;
using System.Text.Json.Serialization;

namespace RPG.Domain.Entities
{
    public class Chapter : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; } = 1;
        public List<Hero> Heroes { get; set; } = new List<Hero>();
        public List<Place> Places { get; set; } = new List<Place>();

        [JsonIgnore]
        public Story Story { get; set; } = default!;

        public List<Session> Sessions { get; set; } = new List<Session>();
        public List<Link> Links { get; set; } = new List<Link>();
        public bool Draft { get; set; }
        }
}