using Communication.Domain;
using Drive.API.External.Cloudflare;
using Events.Domain;
using RPG.Infrastructure.External.Firebase;

namespace Connector
{
    /// <summary>
    /// Represents the shape of the application-specific "config" section.
    /// This class is informational only and is not used for runtime binding.
    /// </summary>
    public class ConfigStructure
    {
        /// <summary>
        /// Indicates whether module database migrations should run automatically on startup.
        /// </summary>
        public bool AutoMigrate { get; set; }

        /// <summary>
        /// Determines whether PostgreSQL should be used instead of the default database provider.
        /// </summary>
        public bool UsePostgresql { get; set; }

        /// <summary>
        /// Frontend origins allowed to access the backend from a browser.
        /// </summary>
        public string[] FrontendUrl { get; set; } = [];

        /// <summary>
        /// Firebase configuration used by RPG synchronization features.
        /// </summary>
        public FirebaseOptions? FirebaseOptions { get; set; }

        /// <summary>
        /// Email provider configuration used by the Communication module.
        /// </summary>
        public EmailOptions? EmailOptions { get; set; }

        /// <summary>
        /// Event configuration used to generate external event links.
        /// </summary>
        public EventOptions? EventOptions { get; set; }

        /// <summary>
        ///     Cloudflare configuration
        /// </summary>
        public CloudflareOptions? CloudflareOptions { get; set; }

        /// <summary>
        ///     Default storage for media
        /// </summary>
        public required string DefaultStorage { get; set; }
    }
}
