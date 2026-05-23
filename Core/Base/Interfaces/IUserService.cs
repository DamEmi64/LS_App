namespace Base
{
    /// <summary>
    ///     User service
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        ///     List of all register users
        /// </summary>
        IEnumerable<UserData> Users { get; }
    }
}
