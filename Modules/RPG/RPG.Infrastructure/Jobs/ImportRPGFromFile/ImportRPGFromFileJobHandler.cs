using Base;
using RPG.Domain.Dictionaries;
using RPG.Domain.Repositories;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class ImportRPGFromFileJobHandler : JobHandler<ImportRPGFromFileJob>
    {
        private readonly IStoryRepository _storyRepository;
        private readonly List<IRPGDataConverter> _rpgDataConverters;

        public ImportRPGFromFileJobHandler(
            IJobContext jobContext,
            IStoryRepository storyRepository,
            IEnumerable<IRPGDataConverter> rpgDataConverters)
            : base(jobContext)
        {
            _storyRepository = storyRepository;
            _rpgDataConverters = rpgDataConverters.ToList();
        }

        public override async Task Execute(ImportRPGFromFileJob request)
        {
            var converter = _rpgDataConverters.FirstOrDefault(x=>x.Type == request?.Model?.Type);

            if (request.Model?.Type == RPGFileTypes.Firebase && request.Model is not null)
            {
                request.Model.FileContent = request.Model.ExternalUrl;
            }

            if (converter is not null && !string.IsNullOrEmpty(request.Model?.FileContent))
            {
                var story = await converter.Convert(request.Model.FileContent);

                if (story is not null)
                {
                    await _storyRepository.Add(story);
                }
            }
        }
    }
}