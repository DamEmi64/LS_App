using Base;
using Base.Connect;

namespace SharedEvents
{
    public record LaterEvent(NotifyEvent Data, DateTime Date) : IEvent
    {
    }

}
