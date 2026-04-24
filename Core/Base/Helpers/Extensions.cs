using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Base
{
    /// <summary>
    /// Provides a set of helper and extension methods for configuring services,
    /// working with collections, and handling common application utilities.
    /// </summary>
    public static class Extensions
    {
        private const string UsePostgresql = "usePostgresql";

        /// <summary>
        /// Wraps a single object into a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="o">The object to wrap.</param>
        /// <returns>A list containing the provided object.</returns>
        public static List<T> ToSingleItemList<T>(this T o)
            => new()
            { o };

        /// <summary>
        /// Registers a database context in the dependency injection container.
        /// </summary>
        /// <typeparam name="T">The type of the DbContext.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="connString">The database connection string.</param>
        /// <param name="usePostgreSQL">Indicates whether PostgreSQL should be used instead of SQL Server.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, string connString, bool usePostgreSQL = false)
            where T : DbContext, IDbContextBase
        {
            if (usePostgreSQL)
            {
                return services.AddDbContext<T>(o =>
                {
                    o.UseNpgsql(connString);
                });
            }

            return services.AddDbContext<T>(o =>
            {
                o.UseSqlServer(connString);
                o.ReplaceService<IHistoryRepository, MigrationHistoryRepository>();
            });
        }

        /// <summary>
        /// Registers a database context in the dependency injection container.
        /// </summary>
        /// <typeparam name="T">The type of the DbContext.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="connString">The database connection string.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddDatabase<T>(this IServiceCollection services, string connString)
            where T : DbContext, IDbContextBase
            => AddDatabase<T>(services, connString, AppConfiguration.Instance.GetValue(UsePostgresql, false));

        /// <summary>
        /// Registers a notifier implementation in the dependency injection container.
        /// </summary>
        /// <typeparam name="T">The type of the notifier instance.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddNotifier<T>(this IServiceCollection services)
            where T : class, INotifierInstance
            => services.AddScoped<INotifierInstance, T>();

        /// <summary>
        /// Converts a file extension into its corresponding MIME content type.
        /// </summary>
        /// <param name="extension">The file extension (with or without leading dot).</param>
        /// <returns>The corresponding MIME type, or "application/octet-stream" if unknown.</returns>
        public static string ToContentType(this string extension) =>
            extension.ToLower().Replace(".", string.Empty) switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "bmp" => "image/bmp",
                "pdf" => "application/pdf",
                "txt" => "text/plain",
                "doc" or "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                "xls" or "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "html" or "htm" => "application/html",
                _ => "application/octet-stream",
            };

        /// <summary>
        ///     Provides basic job operation
        /// </summary>
        /// <param name="id">Operation id</param>
        /// <param name="name">Operation title</param>
        /// <param name="queue">Operation queue</param>
        /// <returns>Job operation</returns>
        public static Operation Operation(int id, string name, string queue) => new() { Id = id, Name = name, Queue = queue };


        /// <summary>
        ///     Generates module information
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="module"></param>
        /// <returns></returns>
        public static ModuleInfo Info<T>(this T module) where T : IModule => new() { Name = module.Name, Version = module.Version, Module = module };

        public static bool IsImage(this Media media)
            => media is not null && !string.IsNullOrWhiteSpace(media.Extension) &&
            (
                media.Extension.Equals("jpg", StringComparison.OrdinalIgnoreCase) ||
                media.Extension.Equals("jpeg", StringComparison.OrdinalIgnoreCase) ||
                media.Extension.Equals("png", StringComparison.OrdinalIgnoreCase) ||
                media.Extension.Equals("gif", StringComparison.OrdinalIgnoreCase) ||
                media.Extension.Equals("bmp", StringComparison.OrdinalIgnoreCase) ||
                media.Extension.Equals("webp", StringComparison.OrdinalIgnoreCase)
            );

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
                .GetRequiredService<IConnectorResolver>();

            foreach (var module in modules)
            {
                var existing = connector.Modules
                    .FirstOrDefault(x => x.Name == module.Name);

                if (existing is null)
                {
                    throw new ModuleInfoEx.NeccessaryModuleNeededException(module.Name);
                }

                if (module.Version is not null)
                {
                    if (!Matches(existing.Version, module.Version))
                    {
                        throw new ModuleInfoEx.ModuleVersionInvalidException(module.Name, existing.Version, module.Version);
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
