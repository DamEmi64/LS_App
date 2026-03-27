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

        public async Task Execute(IJobContext jobContext)
        {
            var byteArray = Convert.FromBase64String(FileData);

            var path = Locaction;

            bool exists = Directory.Exists(path);

            if (!exists)
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllBytesAsync(path, byteArray);
        }
    }
}