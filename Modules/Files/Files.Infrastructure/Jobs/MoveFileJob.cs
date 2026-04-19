using Base;
using Files.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

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

        public async Task Execute(IJobContext jobContext)
        {
            var httpFactory = jobContext.Resolve<IHttpClientFactory>();
            var repo = jobContext.Resolve<IFileRepository>();

            await ExecuteInternal(httpFactory, repo, jobContext);
        }

        private async Task ExecuteInternal(IHttpClientFactory httpClientFactory, IFileRepository fileRepository, IJobContext jobContext)
        {
            var atr = System.IO.File.GetAttributes(Source);

            if ((atr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(Destination)))
                {
                    System.IO.Directory.CreateDirectory(Destination);
                }

                if (!Destination.EndsWith("\\"))
                {
                    Destination += "\\";
                }

                var files = Directory.GetFiles(Source);
                foreach (var filepath in files)
                {
                    var file = Path.GetFileName(filepath);
                    System.IO.File.Move(filepath, Destination + file);
                }
            }
            else
            {
                System.IO.File.Move(Source, Destination);
            }

            var entity = await fileRepository.Get(FileId);

            if (entity is not null)
            {
                entity.Locaction = Destination;
                await fileRepository.Update(entity);
            }
        }
    }
}