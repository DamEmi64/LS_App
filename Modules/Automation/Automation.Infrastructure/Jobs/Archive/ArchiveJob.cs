using Base;
using System.IO.Compression;

namespace Automation.Infrastructure.Jobs
{
    public class ArchiveJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public string Name => $"Archive {SourceDir} to {DestDir}";

        public required string SourceDir { get; set; }

        public required string DestDir { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.ArchiveData;

        public async Task Execute(IJobContext jobContext)
        {

        }
    }
}