using FluentResults;

namespace Communication.Infrastructure.Connect.SendEmail.Strategies
{
    public interface ISendStrategy
    {
        string Mode { get; }
        Task<Result> Send(string to, string subject, string body, string? from = null, string? messageId = null);
    }
}