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
}