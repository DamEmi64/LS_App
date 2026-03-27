using Base;

namespace Files.Infrastructure.Jobs
{
    public class DeleteFileJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Delete files from {Locaction}";

        public required string Locaction { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.DeleteFile;

        public async Task Execute(IJobContext jobContext)
        {
            var atr = File.GetAttributes(Locaction);

            if ((atr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                Directory.Delete(Locaction, true);
            }
            else
            {
                File.Delete(Locaction);
            }
        }
    }
}