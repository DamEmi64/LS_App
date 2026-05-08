using Microsoft.AspNetCore.SignalR;

namespace RPG.Infrastructure.Hubs
{
    public class RPGHub : Hub
    {
        public async Task ChangeVideo(object title)
        {
            await Clients.All.SendAsync("VideoChanged", title);
        }

        public async Task UpdateBattleState(List<object> npcs)
        {
            await Clients.All.SendAsync("BattleStateChanged", npcs);
        }

        public async Task ChangeBackground(string background)
        {
            await Clients.All.SendAsync("BackgroundChanged", background);
        }
    }
}
