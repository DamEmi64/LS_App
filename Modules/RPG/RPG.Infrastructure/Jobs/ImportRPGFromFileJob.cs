using Base;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Repositories;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class ImportRPGFromFileJob : IJob
    {
        public int OperationId => Domain.Dictionaries.Operations.ImportRPGFromFile;

        public Guid Id { get; set; }

        public List<IJob> Children { get; set; } = new List<IJob>();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Get {RPGName} from file";

        public string FileContent { get; set; } = string.Empty;

        public string RPGName { get; set; } = string.Empty;

        public FileConverterType ConverterType { get; set; }

        public async Task Execute(IJobContext jobContext)
        {
            var storyRepo = jobContext.ServiceProvider.GetRequiredService<IStoryRepository>();
            var fileConverters = jobContext.ServiceProvider.GetServices<IFileConverter>();

            var converter = fileConverters.FirstOrDefault(c => c.Type == ConverterType);

            if (converter is not null)
            {
                var story = await converter.Convert(FileContent) ?? null;

                if (story is not null)
                {
                    await storyRepo.Add(story);
                }
            }
        }
    }
}
