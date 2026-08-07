using FluentResults;
using MediatR;

namespace Base.Connect
{
    public interface IEvent<T> : IRequest<Result<T>> where T : class?;
    public interface IEvent : IRequest<Result>;
}
