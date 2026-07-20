using Base;
using Events.Domain.Entities;
using Events.Infrastructure.Jobs;

namespace Events.Infrastructure.Services.InvitationService
{
    public class InvitationService : IInvitationService
    {
        private readonly IJobEngine _jobEngine;

        public InvitationService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task SendInvitation(Event eventData, IEnumerable<UserData> usersToSend, UserData currentUser)
        {
            var schema = _jobEngine.Create($"Sending invitation to {eventData.Title}");

            foreach (var user in usersToSend)
            {
                schema.AddJob(new SendInvitationJob
                {
                    Event = eventData,
                    Receiver = user
                });
            }

            await _jobEngine.Execute(schema, currentUser);
        }
    }
}
