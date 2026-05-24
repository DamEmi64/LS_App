using Base;
using FluentResults;
using MediatR;

namespace Base;

public record ProvideBasicRoles(List<PermissionInfo> Permissions) : IRequest<Result>;
