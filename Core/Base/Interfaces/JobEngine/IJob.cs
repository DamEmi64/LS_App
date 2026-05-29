using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Job
/// </summary>
public interface IJob :IRequest<Result>
{
    /// <summary>
    ///     Operation Type
    /// </summary>
    int OperationId { get; }

    /// <summary>
    ///     Identifier 
    /// </summary>
    Guid Id { get; set; }

    /// <summary>
    ///     Children jobs - start when parent ends successfully
    /// </summary>
    List<IJob> Children { get; }

    /// <summary>
    ///     Name
    /// </summary>
    string Name { get; }
}