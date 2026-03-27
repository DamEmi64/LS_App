using Base.Helpers;
using RPG.Domain.Entities;

namespace RPG.Application.Dtos
{
    public class StoryDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public List<ChapterDto> Chapters { get; set; } = new List<ChapterDto>();
        public byte[]? Summary { get; set; }
    }
}