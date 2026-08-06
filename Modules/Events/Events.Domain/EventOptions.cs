namespace Events.Domain
{
    public class EventOptions
    {
        public string EventLinkTemplate { get; set; } = "https://localhost:8080/events#{0}";
    }
}
