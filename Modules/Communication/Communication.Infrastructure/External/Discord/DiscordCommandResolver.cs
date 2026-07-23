using CommunicationBase.Attributes;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using System.Reflection;

namespace Communication.Infrastructure.External.Discord
{
    public sealed class DiscordCommandResolver
    {
        private readonly Dictionary<string, DiscordCommandDescriptor> _commands;

        public DiscordCommandResolver()
        {
            _commands = DiscoverCommands();
        }

        public DiscordCommandDescriptor? Resolve(string command)
        {
            _commands.TryGetValue(command, out var descriptor);
            return descriptor;
        }

        public IReadOnlyCollection<DiscordCommandDescriptor> GetCommands()
            => _commands.Values;

        public Dictionary<string, DiscordCommandDescriptor> DiscoverCommands()
        {
            var result = new Dictionary<string, DiscordCommandDescriptor>(
                StringComparer.OrdinalIgnoreCase);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsAbstract)
                        continue;

                    if (!type.IsAssignableTo(typeof(IDiscordCommandsWrapper)))
                        continue;

                    foreach (var method in type.GetMethods(
                                 BindingFlags.Instance |
                                 BindingFlags.Public))
                    {
                        var attribute = method.GetCustomAttribute<DiscordCommandAttribute>();

                        if (attribute == null)
                            continue;

                        if (method.ReturnType != typeof(Task<DiscordResponse>))
                            throw new InvalidOperationException(
                                $"{type.Name}.{method.Name} must return Task<DiscordResponse>.");

                        var parameters = method.GetParameters();

                        if (parameters.Length != 1)
                            throw new InvalidOperationException(
                                $"{type.Name}.{method.Name} must have exactly one parameter.");

                        if (parameters[0].ParameterType != typeof(DiscordCommandContext))
                            throw new InvalidOperationException(
                                $"{type.Name}.{method.Name} parameter must be of type {nameof(DiscordCommandContext)}.");

                        if (!result.TryAdd(attribute.Command,
                                new DiscordCommandDescriptor(
                                    attribute.Command,
                                    type,
                                    method,
                                    attribute.Configuration ?? "{}",
                                    attribute.Arguments ?? new Dictionary<string, bool>())))
                        {
                            throw new InvalidOperationException(
                                $"Duplicate Discord command '{attribute.Command}'.");
                        }
                    }
                }
            }

            return result;
        }
    }
}
