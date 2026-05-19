namespace Base.Helpers
{
    /// <summary>
    ///     Default entity filter
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityFilter<T>
    {
        /// <summary>
        ///     Page size
        /// </summary>
        int PageSize { get; }

        /// <summary>
        ///     Page
        /// </summary>
        int Page { get; }

        /// <summary>
        ///     Filter entity
        /// </summary>
        /// <param name="data"></param>
        /// <param name="count">number of filtered elements(ignore page size)</param>
        /// <returns></returns>
        IEnumerable<T> Filter(IEnumerable<T> data, out int? count);
    }
}