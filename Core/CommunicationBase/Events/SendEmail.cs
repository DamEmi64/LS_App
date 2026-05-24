using FluentResults;
using MediatR;

namespace CommunicationBase.Events
{
    /// <summary>
    ///     Sending email event
    /// </summary>
    /// <param name="To"></param>
    /// <param name="Subject"></param>
    /// <param name="Body"></param>
    /// <param name="From"></param>
    public record SendEmail (
        string To,
        string Subject,
        string Body,
        string? From = null
    ) : IRequest<Result>;
}
