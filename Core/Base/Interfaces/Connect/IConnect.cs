using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Responsible for module communication
/// </summary>
public interface IConnect
{
    /// <summary>
    ///     Sending data between modules
    /// </summary>
    /// <typeparam name="T1">The type of the data being sent</typeparam>
    /// <typeparam name="T2">The type of the response expected</typeparam>
    /// <param name="data">The data to be sent</param>
    /// <returns>The response from the receiving module</returns>
    Task<Result<T2>> Send<T1, T2>(T1 data) where T1 : IRequest<Result<T2>>;

    /// <summary>
    ///     Sending data between modules
    /// </summary>
    /// <typeparam name="T1">The type of the data being sent</typeparam>
    /// <typeparam name="T2">The type of the response expected</typeparam>
    /// <param name="data">The data to be sent</param>
    /// <returns>The response from the receiving module</returns>
    Task<Result> Send<T1>(T1 data) where T1 : IRequest<Result>;
}
