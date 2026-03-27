namespace Base.Helpers
{
    /// <summary>
    ///     Default entity filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityFilter<T>
    {
        /// <summary>
        ///     Filter entity
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        IEnumerable<T> Filter(IEnumerable<T> data);
    }
}