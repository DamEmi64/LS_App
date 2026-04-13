namespace Base
{
    /// <summary>
    ///     Job
    /// </summary>
    public interface IJob
    {
        /// <summary>
        ///     Operation Type
        /// </summary>
        int OperationId { get; }

        /// <summary>
        ///     Identifier 
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        ///     Children jobs - start when parent ends successfully
        /// </summary>
        List<IJob> Children { get; }

        /// <summary>
        ///     Request date
        /// </summary>
        DateTimeOffset RequestDate { get; }

        /// <summary>
        ///     Name
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Execution method
        /// </summary>
        /// <param name="jobContext">Job context</param>
        /// <returns></returns>
        Task Execute(IJobContext jobContext);
    }

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
        ///     Service provider
        /// </summary>
        IServiceProvider ServiceProvider { get; }

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
    }
}