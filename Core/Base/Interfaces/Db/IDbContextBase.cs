namespace Base
{
    /// <summary>
    ///     Default database context
    /// </summary>
    public interface IDbContextBase
    {
        /// <summary>
        ///     Name of the database context.
        /// </summary>
        string ContextName { get; }
    }
}