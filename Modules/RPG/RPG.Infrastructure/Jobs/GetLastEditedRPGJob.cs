using Base;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GetLastEditedRPGJob : IJob
    {
        public int OperationId => Domain.Dictionaries.Operations.GetLastRPG;

        public Guid Id { get; set; }

        public List<IJob> Children { get; set; } = new List<IJob>();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => "Get last edited RPG";

        public async Task Execute(IJobContext jobContext)
        {
            var storyRepo = jobContext.ServiceProvider.GetRequiredService<IStoryRepository>();
            var lastEdited = await storyRepo.GetLastEdited();
            ArgumentNullException.ThrowIfNull(lastEdited);

            var storyExtended = lastEdited.ToModel();
            jobContext.PassData(storyExtended);
        }
    }
}
