using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Get list of all register users
/// </summary>
public record GetUsers() : IRequest<Result<List<UserData>>>;
