using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Base
{
    public class ModuleInfo
    {
        public required string Name { get; set; }
        public required string Version { get; set; }
        public required IModule Module { get; set; }
    }

    public class NeccessaryModuleNeededException : Exception
    {
        public NeccessaryModuleNeededException(string module)
            : base($"Module '{module}' is required but was not found.")
        {

        }
    }

    public class ModuleVersionInvalidException : Exception
    {
        public ModuleVersionInvalidException(string module, string currentVersion, string moduleVersion)
            : base($"Module '{module}' version '{currentVersion}' is lower than required '{moduleVersion}'.")
        {
        }
    }


    public static class ModuleExtensions
    {
        public static ModuleInfo Info<T>(this T module) where T : IModule => new() { Name = module.Name, Version = module.Version, Module = module };

        /// <summary>
        /// Validates required modules against the connector.
        /// Ensures that each module exists and meets the minimum version requirement.
        /// </summary>
        /// <param name="applicationBuilder">The application builder.</param>
        /// <param name="modules">The modules to validate and use.</param>
        /// <returns>The application builder for chaining.</returns>
        /// <exception cref="NeccessaryModuleNeededException">
        /// Thrown when a required module is missing
        /// </exception>
        /// <exception cref="ModuleVersionInvalidException">
        /// Thrown when a required module version is too low
        /// </exception>
        public static IApplicationBuilder UseModules(
            this IApplicationBuilder applicationBuilder,
            params ModuleInfo[] modules)
        {
            if (applicationBuilder == null)
                throw new ArgumentNullException(nameof(applicationBuilder));

            var connector = applicationBuilder.ApplicationServices
                .GetRequiredService<IConnector>();

            foreach (var module in modules)
            {
                var existing = connector.Modules
                    .FirstOrDefault(x => x.Name == module.Name);

                if (existing is null)
                {
                    throw new NeccessaryModuleNeededException(module.Name);
                }

                if (module.Version is not null)
                {
                    if (!Matches(existing.Version, module.Version))
                    {
                        throw new ModuleVersionInvalidException(module.Name, existing.Version, module.Version);
                    }
                }
            }

            return applicationBuilder;
        }

        private static bool Matches(string actual, string required)
        {
            if (string.IsNullOrWhiteSpace(actual) || string.IsNullOrWhiteSpace(required))
                return false;

            var actualParts = actual.Split('.');
            var requiredParts = required.Split('.');

            for (int i = 0; i < requiredParts.Length; i++)
            {
                if (i >= actualParts.Length)
                    return false;

                var req = requiredParts[i];
                var act = actualParts[i];

                if (req.Equals("x", StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!req.Equals(act, StringComparison.OrdinalIgnoreCase))
                    return false;
            }

            return true;
        }
    }
}
