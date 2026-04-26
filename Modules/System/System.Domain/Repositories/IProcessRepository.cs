using Base;
using System.Domain.Entities;

namespace System.Domain.Repositories
{
    public interface IProcessRepository : IEntityRepository<Process>
    {
        Task AddError(Guid processId, string jobId, string message);

        bool CheckIfEnded(Guid processId, Guid[] jobIds);

        IEnumerable<ProcessMilestone> GetActiveMilestones();

        Task AddMilestones(IEnumerable<ProcessMilestone> milestones);

        string? GetHangfireJobId(Guid jobId);

        Task<ProcessRead?> GetReadData(Guid processId);
        IEnumerable<ProcessRead> GetAllReadData();
    }
}