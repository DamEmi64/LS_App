using FluentResults;
using MediatR;

namespace Base
{
    public record GetUserByLogin(string login) : IRequest<Result<UserData?>>;
}
