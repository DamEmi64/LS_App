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
        IProcessSchema Create(string title);

        /// <summary>
        ///     Create process and execute using process schema and user data
        /// </summary>
        /// <param name="schema"></param>
        /// <param name="userData"></param>
        /// <returns></returns>
        Task Execute(IProcessSchema schema, UserData userData);
    }
}