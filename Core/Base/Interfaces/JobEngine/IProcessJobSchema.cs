namespace Base
{
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