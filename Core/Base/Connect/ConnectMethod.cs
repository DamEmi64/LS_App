using Base.Connect;
using FluentResults;
using FluentValidation;
using MediatR;

namespace Base;

/// <summary>
///     Connect method for events that return a result with a response.
/// </summary>
public abstract class ConnectMethod<TEvent, TResponse>
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
///     Connect method for events with validation that return a result with a response.
/// </summary>
public abstract class ConnectMethod<TEvent, TValidator, TResponse>
    : IRequestHandler<TEvent, Result<TResponse>>
    where TEvent : IEvent<TResponse>
    where TResponse : class?
    where TValidator : IValidator<TEvent>
{
    public abstract Task<TResponse> HandleAsync(
        TEvent request,
        CancellationToken cancellationToken);

    public Task<Result<TResponse>> Handle(
        TEvent request,
        CancellationToken cancellationToken)
    {
        var validator = Activator.CreateInstance<TValidator>();
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            return Task.FromResult(Result.Fail<TResponse>(validationResult.Errors.Select(e => e.ErrorMessage)));
        }

        return Result.Try(() => HandleAsync(request, cancellationToken));
    }
}

/// <summary>
///     Connect method for events.
/// </summary>
public abstract class ConnectMethod<TEvent>
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
