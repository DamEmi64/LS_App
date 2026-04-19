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

    public static class OperationExtensions
    {
        public static Operation Operation(int id, string name, string queue)
            => new()
            { Id = id, Name = name, Queue = queue };
    }
}