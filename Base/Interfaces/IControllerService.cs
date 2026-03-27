using Microsoft.AspNetCore.Http;

namespace Base
{
    /// <summary>
    ///     Controller service interface
    /// </summary>
    public interface IControllerService
    {
        /// <summary>
        ///    Get current user as user data
        /// </summary>
        /// <returns></returns>
        public Task<UserData?> GetCurrentUser();

        /// <summary>
        ///    Get user data from http context
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public Task<UserData?> GetUser(HttpContext context);

        /// <summary>
        ///    Notifier instance
        /// </summary>
        public INotifier Notifier { get; }
    }
}