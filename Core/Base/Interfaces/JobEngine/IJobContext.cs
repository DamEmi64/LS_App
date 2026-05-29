namespace Base
{
    public interface IJobContext
    {
        /// <summary>
        ///     Job identifier
        /// </summary>
        Guid Id { get; }

        /// <summary>
        ///     Hangfire job identifier
        /// </summary>
        string JobId { get; }

        /// <summary>
        ///     Add logs
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task AddLog(string log);

        /// <summary>
        ///     Add Error
        /// </summary>
        /// <param name="error">Error message</param>
        /// <returns></returns>
        Task AddError(string error);

        /// <summary>
        ///     Cache job data (pass data between jobs)
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        void PassData<T>(T data);

        /// <summary>
        ///     Get cached job data
        /// </summary>
        /// <returns></returns>
        T? GetData<T>();

        /// <summary>
        ///     Method invoke before execution
        /// </summary>
        /// <returns></returns>
        Task OnStart();

        /// <summary>
        ///     Method invoke after execution
        /// </summary>
        /// <returns></returns>
        Task OnComplete();
    }
}