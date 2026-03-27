using Microsoft.Extensions.Configuration;

namespace Base
{
    /// <summary>
    /// Provides centralized access to application configuration values,
    /// including strongly-typed sections, connection strings, and common settings.
    /// </summary>
    public static class AppConfiguration
    {
        private const string CONFIG = "config";
        private const string VERSION = "version";
        private const string DEFAULT_DB = "DbContext";

        private static IConfiguration _configuration = default!;

        /// <summary>
        /// Gets the root configuration section used for application-specific settings.
        /// </summary>
        public static IConfigurationSection Instance => _configuration.GetSection(CONFIG);

        /// <summary>
        /// Gets the root <see cref="IConfiguration"/> instance.
        /// </summary>
        public static IConfiguration Root => _configuration;

        /// <summary>
        /// Gets a configuration section using the name of the specified type.
        /// </summary>
        /// <typeparam name="T">The type whose name will be used as the section key.</typeparam>
        /// <returns>The corresponding configuration section.</returns>
        public static IConfigurationSection Get<T>() => Get(nameof(T));

        /// <summary>
        /// Gets a configuration section by key from the main configuration section.
        /// </summary>
        /// <param name="key">The configuration section key.</param>
        /// <returns>The corresponding configuration section.</returns>
        public static IConfigurationSection Get(string key) => _configuration.GetSection(CONFIG).GetSection(key);

        /// <summary>
        /// Gets a connection string by key.
        /// </summary>
        /// <param name="key">The name of the connection string.</param>
        /// <returns>The connection string if found; otherwise, null.</returns>
        public static string? GetConnectionString(string key) => _configuration.GetConnectionString(key);

        /// <summary>
        /// Gets a configuration value of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <returns>The value if found; otherwise, the default value of the type.</returns>
        public static T? GetValue<T>(string key) => _configuration.GetSection(CONFIG).GetValue<T>(key);

        /// <summary>
        /// Gets a configuration value of the specified type, returning a default value if not found.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="key">The configuration key.</param>
        /// <param name="defaultValue">The default value to return if the key is not found.</param>
        /// <returns>The configuration value or the provided default value.</returns>
        public static T GetValue<T>(string key, T defaultValue) =>
            _configuration.GetSection(CONFIG).GetValue(key, defaultValue) ?? defaultValue;

        /// <summary>
        /// Gets the default database connection string.
        /// </summary>
        /// <exception cref="NullReferenceException">
        /// Thrown when the default connection string is not defined.
        /// </exception>
        public static string DefaultConnectionString =>
            GetConnectionString(DEFAULT_DB) ?? throw new NullReferenceException();

        /// <summary>
        /// Gets the application version from configuration.
        /// </summary>
        public static string Version =>
            _configuration.GetValue<string>(VERSION) ?? "(unknown)";

        /// <summary>
        /// Initializes the configuration provider.
        /// This method must be called before accessing configuration values.
        /// </summary>
        /// <param name="configuration">The application configuration instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when configuration is null.</exception>
        public static void Initialize(IConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            _configuration = configuration;
        }
    }
}
