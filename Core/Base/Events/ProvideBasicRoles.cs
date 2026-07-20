using FluentResults;
using MediatR;

namespace Base;

/// <summary>
///     Ensure the basic roles exists and contains the supplied permissions.
/// </summary>
public record ProvideBasicRoles(List<PermissionInfo> Permissions) : IRequest<Result>;
