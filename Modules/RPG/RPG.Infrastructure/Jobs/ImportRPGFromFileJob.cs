using Base;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Dictionaries;
using RPG.Domain.Repositories;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class ImportRPGFromFileJob : IJob
    {
        public int OperationId => Domain.Dictionaries.Operations.ImportRPGFromFile;

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new List<IJob>();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Get {Model?.Title ?? string.Empty} from file";

        public ImportRPGModel? Model { get; set; }

        public async Task Execute(IJobContext jobContext)
        {
            var storyRepo = jobContext.ServiceProvider.GetRequiredService<IStoryRepository>();
            var fileConverters = jobContext.ServiceProvider.GetServices<IRPGDataConverter>();

            var converter = fileConverters.FirstOrDefault(c => c.Type == Model?.Type);

            if (Model?.Type == RPGFileTypes.Firebase && Model is not null)
            {
                Model.FileContent = Model.ExternalUrl;
            }

            if (converter is not null && !string.IsNullOrEmpty(Model?.FileContent))
            {
                var story = await converter.Convert(Model.FileContent) ?? null;

                if (story is not null)
                {
                    await storyRepo.Add(story);
                }
            }
        }
    }
}
