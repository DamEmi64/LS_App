using Base.Connect;

namespace Base;

/// <summary>
///     Get list of all register users
/// </summary>
public record GetUsers() : IEvent<List<UserData>>;
