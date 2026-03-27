using RPG.Domain.Entities;

namespace RPG.Application.Dtos
{
    public class PlaceDto
    {
        public Guid? Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Guid? Chapter { get; set; }
        public string? Image { get; set; }
        public Guid? ImageId { get; set; }

        public Place ToEntity()
        {
            return new Place
            {
                Chapter = Chapter.HasValue ? new Chapter { Id = Chapter.Value } : new Chapter(),
                Description = Description,
                Id = Id ?? Guid.NewGuid(),
                Title = Title,
                Image = ImageId.HasValue ? ImageId.Value : null
            };
        }
    }
}