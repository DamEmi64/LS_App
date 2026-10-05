using Base;
using FluentResults;

namespace SharedEvents
{
    public static class Extensions
    {
        public static Task<Result<List<UserData>>> GetUsers(this IConnect connectClient)
=> connectClient.Send<GetUsers, List<UserData>>(new GetUsers());

        public static Task ProvideBasicRoles(this IConnect connectClient, List<PermissionInfo> permissions)
            => connectClient.Send(new ProvideBasicRoles(permissions));

        public static Task<Result<UserData?>> GetUserIdByLogin(this IConnect connectClient, string Login)
                => connectClient.Send<GetUserByLogin, UserData?>(new GetUserByLogin(Login));
    }
}
