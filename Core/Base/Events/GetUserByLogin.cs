using Base.Connect;
using FluentResults;
using MediatR;

namespace Base
{
    public record GetUserByLogin(string login) : IEvent<UserData?>;
}
