using System.Reflection;

namespace Communication.Infrastructure.External.Discord
{
    public sealed class DiscordCommandDescriptor
    {
        public string Command { get; }

        public Type CommandClass { get; }

        public MethodInfo Method { get; }

        public string DefaultConfiguration { get; }
        public Dictionary<string,bool> Arguments { get; }

        public DiscordCommandDescriptor(
            string command,
            Type commandClass,
            MethodInfo method,
            string defaultConfiguration,
            Dictionary<string, bool> arguments)
        {
            Command = command;
            CommandClass = commandClass;
            Method = method;
            DefaultConfiguration = defaultConfiguration;
            Arguments = arguments;
        }
    }
}
