using Base;

namespace Files.Infrastructure.Jobs
{
    public class CopyFileJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Copy files from {Source} to {Destination}";

        public required string Destination { get; set; }

        public required string Source { get; set; }
        public Guid FileId { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.CopyFile;

        public async Task Execute(IJobContext jobContext)
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
                    System.IO.File.Copy(filepath, Destination + file);
                }
            }
            else
            {
                System.IO.File.Copy(Source, Destination);
            }
        }
    }
}