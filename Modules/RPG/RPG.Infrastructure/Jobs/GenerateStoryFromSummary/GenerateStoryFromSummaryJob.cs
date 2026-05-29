using Base;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateStoryFromSummaryJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Convert {Summary?.Title ?? string.Empty} to story model";

        public SummaryModel? Summary { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateStoryFromSummary;
    }
}