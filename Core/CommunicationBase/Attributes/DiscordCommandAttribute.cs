namespace CommunicationBase.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class DiscordCommandAttribute : Attribute
    {
        public DiscordCommandAttribute(string command, string? configuration = null)
        {
            Command = command;
            Configuration = configuration;
        }

        public string Command { get; }
        public string? Configuration { get; }
    }
}
