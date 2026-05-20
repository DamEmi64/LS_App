using Base;
using Events.Domain.Entities;

namespace Events.Infrastructure.Services.ReminderService
{
    public interface IReminderService
    {
        Task AddReminder(DateTimeOffset remindAt, Event eventData, UserData currentUser);
        Task RemoveReminder(Event eventData);
    }
}