using Base;
using Communication.Domain.Entities;

namespace Communication.Domain.Repositories
{
    public interface IDiscordRepository : IEntityRepository<DiscordCmd>
    {
        Task<DiscordCmd?> GetCmd(string cmd);
        bool Exist(string cmd);
        Task DisableAllCommands();
        Task Enable(string cmd);
        Task Disable(string cmd);
    }
}
