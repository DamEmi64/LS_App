using Base;
using Base.Connect;

namespace SharedEvents;

public record GetUserByLogin(string login) : IEvent<UserData?>;
