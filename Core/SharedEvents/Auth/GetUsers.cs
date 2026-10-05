using Base;
using Base.Connect;

namespace SharedEvents;

/// <summary>
///     Get list of all register users
/// </summary>
public record GetUsers() : IEvent<List<UserData>>;
