using Base.Helpers;

namespace Files.Application.Dtos
{
    public class FileDto
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public Guid? ImageId { get; set; }
        public string? Content { get; set; }
        public string? ImageData { get; set; }
        public string? Locaction { get; set; }
        public int FileType { get; set; }
        public int? GameGenre { get; set; }
        public string? Subject { get; set; }
        public int? Year { get; set; }
        public int? Semester { get; set; }
        public int SourceType { get; set; }
        public string? Links { get; set; }
        public bool? UseFileUpload { get; set; }

        public bool? IsInstall { get; set; }

        public Domain.Entities.File ToEntity()
        => new()
        {
            Id = Guid.NewGuid(),
            Title = Title,
            Image = ImageId,
            Locaction = Locaction,
            FileType = FileType,
            InsDate = DateTimeOffset.Now,
            UpdDate = DateTimeOffset.Now,
            AdditionalData = new Domain.Entities.AdditionalData
            {
                Semester = Semester,
                Subject = Subject,
                GameGenre = GameGenre,
                Id = Guid.NewGuid(),
                InsDate = DateTimeOffset.Now,
                UpdDate = DateTimeOffset.Now,
                Year = Year,
            },
            Sources = Links?.Split("\n").Select(x => new Domain.Entities.SourceLink
            {
                Id = Guid.NewGuid(),
                SourceType = SourceType,
                Link = x,
                InsDate = DateTimeOffset.Now,
                UpdDate = DateTimeOffset.Now,
            }).ToList() ?? new List<Domain.Entities.SourceLink>(),
        };

        public static FileDto ToDto(Domain.Entities.File file)
       => new()
       {
           Id = file.Id,
           Title = file.Title,
           ImageId = file.Image,
           Locaction = file.Locaction,
           FileType = file.FileType,
           Semester = file.AdditionalData?.Semester,
           Subject = file.AdditionalData?.Subject,
           GameGenre = file.AdditionalData?.GameGenre,
           Year = file.AdditionalData?.Year,
           SourceType = file.Sources?.FirstOrDefault()?.SourceType ?? 0,
           Links = string.Join("\n", file.Sources?.Select(x => x.Link) ?? Array.Empty<string>()),
           IsInstall = file.Sources?.Count > 0,
       };
    }
}