namespace Base
{
    public interface IMediaProviderFactory
    {
        /// <summary>
        ///     Create media provider
        /// </summary>
        /// <param name="providerName"></param>
        /// <returns></returns>
        IMediaProvider Create(string? providerName = null);
    }

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
        /// Load multiple media content by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="removeWebsiteExtras">Removes additional website data</param>
        /// <returns></returns>
        IAsyncEnumerable<Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras = false);

        /// <summary>
        /// Save media content and return its ID.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id">Media id to override</param>
        /// <param name="extension">File extension (if not specified, it will find one) </param>
        /// <param name="owner">Media owner (user id), if null, media is public and can be accessed by anyone with the link.</param>
        /// <returns></returns>
        Task<Guid> Save(string content, Guid? id, string? extension = null, string? owner = null);

        /// <summary>
        /// Save media content and return its ID.
        /// </summary>
        /// <param name="content"></param>
        /// <param name="id">Media id to override</param>
        /// <param name="extension">File extension (if not specified, it will find one) </param>
        /// <param name="owner">Media owner (user id), if null, media is public and can be accessed by anyone with the link.</param>
        /// <returns></returns>
        Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf", string? owner = null);

        /// <summary>
        /// Delete media content by ID.
        /// </summary>
        /// <param name="id">Media id to override</param>
        /// <returns></returns>
        Task Delete(Guid? id);
    }
}