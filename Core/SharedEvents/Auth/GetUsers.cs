using Base;
using FluentResults;
using MediatR;

namespace SharedEvents.Auth
{
    /// <summary>
    ///     Get list of all register users
    /// </summary>
    public record GetUsers() : IRequest<Result<List<UserData>>>;
}
