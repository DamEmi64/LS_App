namespace Base
{
    /// <summary>
    ///     Job operation
    /// </summary>
    public class Operation
    {
        /// <summary>
        ///     Identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///     Operation name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        ///     Operation queue
        /// </summary>
        public required string Queue { get; set; }
    }
}