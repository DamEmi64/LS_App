using Base;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateSummaryJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public string Name => $"Generate summary for {Story?.Title ?? StoryId.ToString()}";

        public Guid StoryId { get; set; }

        public StoryModel? Story { get; set; }

        public bool IsPdf { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateSummary;
    }
}