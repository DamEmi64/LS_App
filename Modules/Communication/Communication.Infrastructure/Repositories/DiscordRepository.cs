using Base;
using Communication.Domain.Entities;
using Communication.Domain.Repositories;
using Communication.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Communication.Infrastructure.Repositories
{
    public class DiscordRepository : EntityRepository<CommunicationContext, DiscordCmd>, IDiscordRepository
    {
        public DiscordRepository(CommunicationContext dbContext) : base(dbContext)
        {
        }

        public bool Exist(string cmd) => DbContext.Set<DiscordCmd>().Any(x => x.Cmd == cmd);

        public Task<DiscordCmd?> GetCmd(string cmd) => DbContext.Set<DiscordCmd>().FirstOrDefaultAsync(x => x.Cmd == cmd);
        public Task Enable(string cmd) => DbContext.Set<DiscordCmd>().Where(x=>x.Cmd == cmd).ExecuteUpdateAsync(x => x.SetProperty(y => y.Active, true));
        public Task Disable(string cmd) => DbContext.Set<DiscordCmd>().Where(x=>x.Cmd == cmd).ExecuteUpdateAsync(x => x.SetProperty(y => y.Active, false));
        public Task DisableAllCommands() => DbContext.Set<DiscordCmd>().ExecuteUpdateAsync(x => x.SetProperty(y => y.Active, false));
    }
}

