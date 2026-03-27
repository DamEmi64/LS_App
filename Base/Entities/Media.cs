namespace Base
{
    /// <summary>
    ///     Media provider
    /// </summary>
    public interface IMediaProvider
    {
        /// <summary>
        /// Load media content by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeWebsiteExtras">Removes additional website data</param>
        /// <returns></returns>
        Task<Media?> Load(Guid id, bool removeWebsiteExtras = false);

        /// <summary>
        /// Save media content and return its ID.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id">Media id to override</param>
        /// <param name="extension">File extension (if not specified, it will find one) </param>
        /// <returns></returns>
        Task<Guid> Save(string content, Guid? id, string? extension = null);

        /// <summary>
        /// Save media content and return its ID.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id">Media id to override</param>
        /// <param name="extension">File extension (if not specified, it will find one) </param>
        /// <returns></returns>
        Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf");

        /// <summary>
        /// Delete media content by ID.
        /// </summary>
        /// <param name="id">Media id to override</param>
        /// <returns></returns>
        Task Delete(Guid? id);
    }

    public class Media : Entity
    {
        public string? ContentStr { get; set; } = string.Empty;

        public byte[]? Content { get; set; }
        public string Extension { get; set; } = "(unknown)";
    }
}