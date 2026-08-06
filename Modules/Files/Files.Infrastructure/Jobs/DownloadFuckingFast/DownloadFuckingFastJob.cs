using Base;
using Files.Domain.Entities;

namespace Files.Infrastructure.Jobs
{
    public class DownloadFuckingFastJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Download file from {Link.Link} to {Locaction}";

        public required string Locaction { get; set; }

        public required SourceLink Link { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.ImportFile;
    }
}