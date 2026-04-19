using FluentResults;

namespace Base.Interfaces
{
    /// <summary>
    ///     Email sender interface
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        ///    Send email asynchronously
        /// </summary>
        /// <param name="to"></param>
        /// <param name="subject"></param>
        /// <param name="body"></param>
        /// <param name="from"></param>
        /// <returns></returns>
        public Task<Result> SendEmailAsync(string to, string subject, string body, string? from = null);
    }
}
