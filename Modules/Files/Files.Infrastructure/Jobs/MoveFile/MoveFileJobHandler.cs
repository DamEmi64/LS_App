using Base;
using Files.Domain.Repositories;
namespace Files.Infrastructure.Jobs.MoveFile
{
    public class MoveFileJobHandler : JobHandler<MoveFileJob>
    {
        private readonly IFileRepository _fileRepository;

        public MoveFileJobHandler(IJobContext jobContext,
            IFileRepository fileRepository)
            : base(jobContext)
        {
            _fileRepository = fileRepository;
        }

        public override async Task Execute(MoveFileJob request)
        {
            var atr = System.IO.File.GetAttributes(request.Source);

            if ((atr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                if (!Directory.Exists(System.IO.Path.GetDirectoryName(request.Destination)))
                {
                    System.IO.Directory.CreateDirectory(request.Destination);
                }

                if (!request.Destination.EndsWith("\\"))
                {
                    request.Destination += "\\";
                }

                var files = Directory.GetFiles(request.Source);
                foreach (var filepath in files)
                {
                    var file = Path.GetFileName(filepath);
                    System.IO.File.Move(filepath, request.Destination + file);
                }
            }
            else
            {
                System.IO.File.Move(request.Source, request.Destination);
            }

            var entity = await _fileRepository.Get(request.FileId);

            if (entity is not null)
            {
                entity.Locaction = request.Destination;
                await _fileRepository.Update(entity);
            }
        }
    }
}
