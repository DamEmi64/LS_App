using Base.Connect;

namespace Base
{
    public record GetUserByLogin(string login) : IEvent<UserData?>;
}
