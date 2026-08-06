using Base.Connect;
using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Base MediatR handler for module event requests
/// </summary>
public abstract class EventHandler<TEvent, TResponse>
    : IRequestHandler<TEvent, Result<TResponse>>
    where TEvent : IEvent<TResponse>
    where TResponse : class?
{
    public abstract Task<TResponse> HandleAsync(
        TEvent request,
        CancellationToken cancellationToken);

    public Task<Result<TResponse>> Handle(
        TEvent request,
        CancellationToken cancellationToken)
        => Result.Try(() => HandleAsync(request, cancellationToken));
}


/// <summary>
///     Base MediatR handler for module event requests
/// </summary>
public abstract class EventHandler<TEvent>
    : IRequestHandler<TEvent, Result>
    where TEvent : IEvent
{
    public abstract Task HandleAsync(
        TEvent request,
        CancellationToken cancellationToken);

    public Task<Result> Handle(
        TEvent request,
        CancellationToken cancellationToken)
        => Result.Try(() => HandleAsync(request, cancellationToken));
}
