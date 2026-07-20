using Base;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GetLastEditedRPGJobHandler : JobHandler<GetLastEditedRPGJob>
    {
        private readonly IStoryRepository _storyRepository;

        public GetLastEditedRPGJobHandler(
            IJobContext jobContext,
            IStoryRepository storyRepository)
            : base(jobContext)
        {
            _storyRepository = storyRepository;
        }

        public override async Task Execute(GetLastEditedRPGJob request)
        {
            var lastEdited = await _storyRepository.GetLastEdited();

            ArgumentNullException.ThrowIfNull(lastEdited);

            var storyExtended = lastEdited.ToModel();

            PassData(storyExtended);
        }
    }
}