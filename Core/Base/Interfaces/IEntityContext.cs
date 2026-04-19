namespace Base
{
    /// <summary>
    ///     Database entity context
    /// </summary>
    public interface IEntityContext
    {
        /// <summary>
        ///     Actual database editor
        /// </summary>
        public string? Editor { get; set; }
    }
}
