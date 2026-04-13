using Api.Setup;
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
    public static class DefaultHelpers
    {
        private const string UsePostgresql = "usePostgresql";

        /// <summary>
        /// Wraps a single object into a <see cref="List{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="o">The object to wrap.</param>
        /// <returns>A list containing the provided object.</returns>
        public static List<T> ToList<T>(this T o)
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
    }
}