using Base;
using Bogus;
using FilesV2.Domain.Repositories;
using SharedEvents;

namespace FilesV2.Infrastructure.Services.NotifyListener
{
    public class FileNotifyListener : INotifyListener
    {
        private readonly IFileRepository _fileRepository;
        private readonly IFolderRepository _folderRepository;

        public FileNotifyListener(IFileRepository fileRepository, IFolderRepository folderRepository)
        {
            _fileRepository = fileRepository;
            _folderRepository = folderRepository;
        }

        public Task Notify<T>(T @event) where T : NotifyEvent
        {
            if (@event is MediaSavedEvent mediaSavedEvent)
            {
                if (mediaSavedEvent.Owner is not null) 
                    return Task.CompletedTask;

                return OnMediaSaved(mediaSavedEvent);
            }
            else if (@event is MediaDeletedEvent mediaDeletedEvent)
            {
                return OnMediaDeleted(mediaDeletedEvent);
            }

            return Task.CompletedTask;
        }

        public async Task OnMediaSaved(MediaSavedEvent mediaSavedEvent)
        {
            var faker = new Faker();
            var dir = await ProvideSystemDirectory();
            var file = await _fileRepository.GetByMediaId(mediaSavedEvent.Id);
            if (file is null)
            {
                await _fileRepository.Add(new Domain.Entities.File
                {
                    Content = mediaSavedEvent.Id,
                    Description = string.Empty,
                    Title = $"{faker.Random.Word()}_{faker.Random.Word()}",
                    Folder = dir,
                    Owner = dir.Owner,
                    Public = true
                });
            }
            else
            {
                file.UpdDate = DateTime.Now;
                await _fileRepository.Update(file);
            }
        }

        public async Task OnMediaDeleted(MediaDeletedEvent mediaDeletedEvent)
        {
            var file = await _fileRepository.GetByMediaId(mediaDeletedEvent.Id);
            if (file != null)
            {
                await _fileRepository.Remove(mediaDeletedEvent.Id);
            }
        }

        private async Task<Domain.Entities.Directory> ProvideSystemDirectory()
        {
            var systemFolder = await _folderRepository.GetSystemFolder();
            if (systemFolder is null)
            {
                systemFolder = new Domain.Entities.Directory
                {
                    Title = "SYSTEM",
                    Owner = new Domain.Entities.CatalogUser
                    {
                        Login = "SYSTEM",
                        UserId = Guid.Empty.ToString()
                    },
                    Public = true
                };
                await _folderRepository.Add(systemFolder);
            }

            return systemFolder;
        }
    }
}
