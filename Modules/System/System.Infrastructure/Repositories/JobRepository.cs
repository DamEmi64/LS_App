using System.Domain.Entities;
using System.Domain.Repositories;
using System.Infrastructure.Db;

namespace System.Infrastructure.Repositories
{
    public class JobRepository : IJobRepository
    {
        private SystemContext _context;

        public JobRepository(SystemContext dbContext, SystemContext context)
        {
            _context = context;
        }

        public async Task Add(Job entity)
        {
            await _context.Jobs.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Job?> Get(Guid id)
        {
            return await _context.Jobs.FindAsync(id);
        }

        public IEnumerable<Job> GetAll()
        {
            return _context.Jobs;
        }

        public async Task Remove(Guid id)
        {
            var job = await Get(id);

            if (job != null)
            {
                _context.Jobs.Remove(job);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(Job entity)
        {
            _context.Jobs.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}