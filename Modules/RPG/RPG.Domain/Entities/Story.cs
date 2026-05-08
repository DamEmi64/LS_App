using Base;
using System.Text.Json.Serialization;

namespace RPG.Domain.Entities
{
    public class Story : Entity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }

        [JsonIgnore]
        public List<Chapter> Chapters { get; set; } = new List<Chapter>();

        public Guid? Summary { get; set; }
        public List<RPGFile> Files { get; set; } = new List<RPGFile>();
    }
}