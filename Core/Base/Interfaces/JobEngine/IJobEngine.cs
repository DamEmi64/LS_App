namespace Base
{
    /// <summary>
    ///     Job Engine
    /// </summary>
    public interface IJobEngine
    {
        /// <summary>
        ///     Create process schema
        /// </summary>
        /// <returns></returns>
        IProcessSchema Create(string title, DateTimeOffset? requestDate = null);

        /// <summary>
        ///     Create process and execute using process schema and user data
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="userData"></param>
        /// <returns>Process id</returns>
        Task<Guid> Execute(IProcessSchema schema, UserData userData);

        /// <summary>
        ///     Cancel Process
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        Task Cancel(Guid processId);
    }
}