using Base;
using Events.Domain.Entities;

namespace Events.Infrastructure.Services.InvitationService
{
    public interface IInvitationService
    {
        Task SendInvitation(Event eventData, IEnumerable<UserData> usersToSend, UserData currentUser);
    }
}