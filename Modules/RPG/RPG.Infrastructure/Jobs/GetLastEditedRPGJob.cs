using Base;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GetLastEditedRPGJob : EventJob
    {
        public override int OperationId => Domain.Dictionaries.Operations.GenerateSummary;

        public override Guid Id { get; set; }

        public override List<IJob> Children { get; set; } = new List<IJob>();

        public override DateTimeOffset RequestDate => DateTimeOffset.Now;

        public override string Name => "Get last edited RPG";

        public override async Task Execute(IJobContext jobContext, object? eventData)
        {
            var storyRepo = jobContext.ServiceProvider.GetRequiredService<IStoryRepository>();
            var lastEdited = await storyRepo.GetLastEdited();
            ArgumentNullException.ThrowIfNull(lastEdited);

            var storyExtended = lastEdited.ToModel();
            jobContext.PassData(storyExtended);
        }
    }
}
