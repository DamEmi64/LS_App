namespace Base
{
    /// <summary>
    ///     Stored media payload with binary or string content and file extension metadata.
    /// </summary>
    public class Media
    {
        /// <summary>
        ///     Metadata id
        /// </summary>
        public Guid Id { get; set; }
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

        /// <summary>
        ///     Media owner (user id)
        ///     If null, media is public and can be accessed by anyone with the link.
        /// </summary>
        public string? Owner { get; set; }
    }
}
