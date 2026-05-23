using Base;
using Events.Domain.Entities;

namespace Events.Extras.Resources
{
    public record EventSendingData(Event Event, UserData User, string? Image, string Link);
}
