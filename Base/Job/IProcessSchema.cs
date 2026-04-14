namespace Base
{
    /// <summary>
    ///     Process schema
    /// </summary>
    public interface IProcessSchema
    {
        /// <summary>
        ///     Add job to current level
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        IProcessJobSchema AddJob(IJob job);

        /// <summary>
        ///     Add milestone that wait for selected jobs to be completed before go on
        ///     Note: selected jobs must be in the current process schema
        /// </summary>
        /// <param name="title"></param>
        /// <param name="jobs"></param>
        /// <returns></returns>
        IProcessSchema AddMilestone(string title, params IJob[] jobs);

        /// <summary>
        ///     Add milestone that wait for the previous job to be completed before go on
        ///     Note: work only for previous jobs declared in schema
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        IProcessSchema AddMilestone(string title);
    }

    public interface IProcessJobSchema : IProcessSchema
    {
        /// <summary>
        ///     Add child job to the current job
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        IProcessJobSchema AddChildJob(IJob job);

        /// <summary>
        ///     Finds job schema for last job with specified operation in this schema branch
        ///     If job not exists, return null
        /// </summary>
        /// <param name="operation">Operation id</param>
        /// <returns></returns>
        IProcessJobSchema? FindLastJobByOperation(int operation);
    }
}