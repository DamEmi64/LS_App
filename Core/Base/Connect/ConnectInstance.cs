using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Base MediatR handler for module requests that return typed data.
/// </summary>
public abstract class ConnectInstance<TRequest, TResponse>
    : IRequestHandler<TRequest, Result<TResponse>>
    where TRequest : IRequest<Result<TResponse>>
{
    public abstract Task<TResponse> HandleAsync(TRequest request);

    public Task<Result<TResponse>> Handle(
        TRequest request,
        CancellationToken cancellationToken)
        => Result.Try(() => HandleAsync(request));
}


/// <summary>
///     Base MediatR handler for module requests that return only a success or failure result.
/// </summary>
public abstract class ConnectInstance<TRequest>
    : IRequestHandler<TRequest, Result>
    where TRequest : IRequest<Result>
{
    public abstract Task HandleAsync(TRequest request);

    public Task<Result> Handle(
        TRequest request,
        CancellationToken cancellationToken)
        => Result.Try(() => HandleAsync(request));
}
