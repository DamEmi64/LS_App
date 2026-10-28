using System.Transactions;
using Microsoft.AspNetCore.Http;

namespace Base;

/// <summary>
/// Wraps state-changing HTTP requests in an ambient database transaction.
/// Module DbContexts enlist when they open their connections.
/// </summary>
public sealed class TransactionMiddleware
{
    private readonly RequestDelegate _next;

    public TransactionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        if (IsReadOnly(context.Request.Method))
        {
            await _next(context);
            return;
        }

        using var transaction = new TransactionScope(
            TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadCommitted,
                Timeout = TransactionManager.DefaultTimeout,
            },
            TransactionScopeAsyncFlowOption.Enabled);

        await _next(context);

        if (context.Response.StatusCode < StatusCodes.Status400BadRequest)
        {
            transaction.Complete();
        }
    }

    private static bool IsReadOnly(string method)
        => HttpMethods.IsGet(method)
            || HttpMethods.IsHead(method)
            || HttpMethods.IsOptions(method)
            || HttpMethods.IsTrace(method);
}
