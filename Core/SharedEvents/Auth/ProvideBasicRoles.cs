using Base;
using FluentResults;
using MediatR;

namespace SharedEvents.Auth
{
    public record ProvideBasicRoles(List<PermissionInfo> Permissions) : IRequest<Result>;
}
