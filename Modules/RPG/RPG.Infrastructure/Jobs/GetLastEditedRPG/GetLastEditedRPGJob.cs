using Base;

namespace RPG.Infrastructure.Jobs
{
    public class GetLastEditedRPGJob : IJob
    {
        public int OperationId => Domain.Dictionaries.Operations.GetLastRPG;

        public Guid Id { get; set; }

        public List<IJob> Children { get; set; } = new();

        public string Name => "Get last edited RPG";
    }
}