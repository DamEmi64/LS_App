using Base;
using Files.Domain.Dictionaries;

namespace Files.Infrastructure.Jobs
{
    public class UploadFileJob : IJob
    {
        public Guid Id { get; set; }
        public Guid FileId { get; set; }
        public required string FileData { get; set; }
        public required string Locaction { get; set; }
        public string Name => $"Upload file to {Locaction}";
        public DateTimeOffset RequestDate => DateTimeOffset.Now;
        public List<IJob> Children { get; set; } = new();

        public int OperationId => Operations.ImportFile;
    }
}