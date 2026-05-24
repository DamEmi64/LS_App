using Base;

namespace Files.Infrastructure.Jobs
{
    public class MoveFileJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Move files from {Source} to {Destination}";

        public required string Destination { get; set; }

        public required string Source { get; set; }
        public Guid FileId { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.MoveFile;
    }
}