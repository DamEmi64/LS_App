using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Db;

namespace System.Infrastructure.Repositories
{
    public class ProcessRepository : IProcessRepository
    {
        private readonly SystemContext _context;

        public ProcessRepository(SystemContext context)
        {
            _context = context;
        }

        public virtual IEnumerable<Process> GetAll()
        {
            return _context.Set<Process>();
        }

        public Task<Process?> Get(Guid id)
        {
            return _context.Set<Process>()
                .Include(x => x.User)
                .Include(x => x.Jobs)
                .Include(x => x.Errors)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task Update(Process entity)
        {
            _context.Processes.Update(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Add(Process entity)
        {
            await _context.Processes.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task Remove(Guid id)
        {
            var entity = await Get(id);

            if (entity is not null)
            {
                _context.Set<Process>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddError(Guid processId, string jobId, string message)
        {
            var process = await Get(processId);

            if (process is null)
            {
                return;
            }

            process.Errors.Add(new ProcessError
            {
                Message = message,
                InsDate = DateTimeOffset.UtcNow,
                JobId = jobId,
                Id = Guid.Empty
            });

            await _context.SaveChangesAsync();
        }

        public bool CheckIfEnded(Guid processId, Guid[] jobIds)
        {
            return _context.Set<Job>().Include(x => x.Process)
                .Where(x => x.Process.Id == processId && jobIds.Contains(x.Id))
                .All(y => y.Status == Base.ProgressStatus.Success);
        }

        public IEnumerable<ProcessMilestone> GetActiveMilestones()
        {
            return _context.Set<ProcessMilestone>().Where(x => !x.Completed);
        }

        public async Task AddMilestones(IEnumerable<ProcessMilestone> milestones)
        {
            _context.Set<ProcessMilestone>().AddRange(milestones);
            await _context.SaveChangesAsync();
        }

        public string? GetHangfireJobId(Guid jobId)
        {
            return _context.Set<Job>().FirstOrDefault(x => x.Id == jobId)?.JobId;
        }

        public Task<ProcessRead?> GetReadData(Guid processId)
        {
            return _context.Set<Process>()
                .AsNoTracking()
                .AsSplitQuery()
                .Include(x => x.User)
                .Include(x => x.Jobs)
                .Include(x => x.Errors)
                .Where(x => x.Id == processId)
                .Select(x => new ProcessRead
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Errors = x.Errors,
                    Status = x.Status,
                    Jobs = x.Jobs,
                    Percentage = x.Percentage,
                    User = x.User
                })
                .FirstOrDefaultAsync();
        }

        public IEnumerable<ProcessRead> GetAllReadData()
        {
            return _context.Set<Process>()
                .Include(x => x.User)
                .AsNoTracking()
                .Select(x => new ProcessRead
                {
                    Id = x.Id,
                    Title = x.Title,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    Status = x.Status,
                    Percentage = x.Percentage,
                    User = x.User
                });
        }
    }
}