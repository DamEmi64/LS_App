using Microsoft.AspNetCore.SignalR;

namespace RPG.Infrastructure.Hubs
{
    public class RPGHub : Hub
    {
        private static string ChapterGroup(Guid chapterId) => $"chapter:{chapterId}";

        public override async Task OnConnectedAsync()
        {
            var chapterId = Context.GetHttpContext()?.Request.Query["chapterId"].ToString();
            if (Guid.TryParse(chapterId, out var id))
                await Groups.AddToGroupAsync(Context.ConnectionId, ChapterGroup(id));

            await base.OnConnectedAsync();
        }

        public async Task ChangeVideo(Guid chapterId, object title)
        {
            await Clients.Group(ChapterGroup(chapterId)).SendAsync("VideoChanged", title);
        }

        public async Task UpdateBattleState(Guid chapterId, List<object> npcs)
        {
            await Clients.Group(ChapterGroup(chapterId)).SendAsync("BattleStateChanged", npcs);
        }

        public async Task ChangeBackground(Guid chapterId, string background)
        {
            await Clients.Group(ChapterGroup(chapterId)).SendAsync("BackgroundChanged", background);
        }
    }
}
