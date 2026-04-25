namespace Base
{
    /// <summary>
    ///     Response list representation
    /// </summary>
    public class ResponseList<T>
    {
        /// <summary>
        ///     Number of items
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        ///     Item list
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();
    }
}
