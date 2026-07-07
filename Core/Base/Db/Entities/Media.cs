namespace Base
{
    /// <summary>
    ///     Stored media payload with binary or string content and file extension metadata.
    /// </summary>
    public class Media : Entity
    {
        /// <summary>
        ///     Content string (js)
        /// </summary>
        public string? ContentStr { get; set; } = string.Empty;

        /// <summary>
        ///     Content
        /// </summary>
        public byte[]? Content { get; set; }

        /// <summary>
        ///     Extension
        /// </summary>
        public string Extension { get; set; } = "(unknown)";
    }
}
