namespace CommunicationBase.Attributes
{
    /// <summary>
    ///   Attribute to mark a method as a Discord command. <br/>
    ///   Command - command name that will be used to invoke the method. This is the string that users will type in Discord to trigger the command.<br/>
    ///   Configuration - optional configuration for the command. This can be used to provide default settings or parameters for the command.<br/>
    ///   Arguments - arguments for the command, write in style {argumentName}:{required or optional} <br/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class DiscordCommandAttribute : Attribute
    {
        public DiscordCommandAttribute(string command, string? configuration = null, params string[] arguments)
        {
            Command = command;
            Configuration = configuration;
            Arguments = arguments.Select(x => x.Split(':')).ToDictionary(x => x[0], x => x[1] == "required");
        }

        /// <summary>
        ///  Command name that will be used to invoke the method. This is the string that users will type in Discord to trigger the command.
        /// </summary>
        public string Command { get; }

        /// <summary>
        ///  Optional configuration for the command. This can be used to provide default settings or parameters for the command.
        /// </summary>
        public string? Configuration { get; }

        /// <summary>
        ///     Arguments for the command. This is a dictionary where the key is the argument name and the value is a boolean indicating whether the argument is required (true) or optional (false).
        /// </summary>
        public Dictionary<string, bool> Arguments { get; }
    }
}
