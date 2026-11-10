using System.Transactions;

namespace System.Infrastructure.JobEngine;

/// <summary>
/// Hangfire uses its own storage transaction and must not enlist in an HTTP request transaction.
/// </summary>
internal static class AmbientTransaction
{
    public static TransactionScope Suppress() => new(
        TransactionScopeOption.Suppress,
        TransactionScopeAsyncFlowOption.Enabled);
}
