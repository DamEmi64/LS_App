using Communication.Domain;
using RPG.Infrastructure.External.Firebase;

namespace Connector
{
    /// <summary>
    /// Configuration structure.
    /// WARNING: Class is not primary used, only for structure summary/info 
    /// </summary>
    public class ConfigStructure
    {
        /// <summary>
        /// Indicates whether automatic database migration is enabled.
        /// </summary>
        public bool AutoMigrate { get; set; }

        /// <summary>
        /// Determines if PostgreSQL should be used as the database provider.
        /// </summary>
        public bool UsePostgresql { get; set; }

        /// <summary>
        /// Firebase-related configuration options.
        /// </summary>
        public FirebaseOptions? FirebaseOptions { get; set; }

        /// <summary>
        /// Email service configuration options.
        /// </summary>
        public EmailOptions? EmailOptions { get; set; }
    }
}
