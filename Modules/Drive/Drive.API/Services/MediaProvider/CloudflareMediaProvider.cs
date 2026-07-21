using Base;
using Drive.API.External.Cloudflare;
using System.Text;

namespace Drive.API.Services;

public class CloudflareMediaProvider : IMediaProvider
{
    // Metadata keys stored as x-amz-meta-* headers on the R2 object.
    private const string ExtensionMetaKey = "extension";
    private const string OwnerMetaKey = "owner";
    private const string JsFormatMetaKey = "jsformat";

    private readonly CloudflareClient _client;

    public CloudflareMediaProvider(CloudflareClient client)
    {
        _client = client;
    }

    private static string KeyFor(Guid id) => id.ToString("N");

    public async Task Delete(Guid? id)
    {
        if (id is null)
        {
            return;
        }

        await _client.DeleteAsync(KeyFor(id.Value));
    }

    public async Task<Media?> Load(Guid id, bool removeWebsiteExtras = false)
    {
        bool exists = await _client.ExistsAsync(KeyFor(id));
        if (!exists)
        {
            return null;
        }

        using var obj = await _client.GetWithMetadataAsync(KeyFor(id));

        obj.Metadata.TryGetValue(ExtensionMetaKey, out var extension);
        obj.Metadata.TryGetValue(OwnerMetaKey, out var owner);
        var isJsFormat = obj.Metadata.TryGetValue(JsFormatMetaKey, out var jsFormatRaw)
                         && bool.TryParse(jsFormatRaw, out var jsFormatParsed)
                         && jsFormatParsed;

        var media = new Media
        {
            Id = id,
            Extension = extension ?? "(unknown)",
            Owner = owner
        };

        if (isJsFormat)
        {
            using var reader = new StreamReader(obj.Content, Encoding.UTF8);
            var contentStr = await reader.ReadToEndAsync();

            media.ContentStr = removeWebsiteExtras
                ? StripWebsiteExtras(contentStr.Decrypt())
                : contentStr.Decrypt();
        }
        else
        {
            using var memoryStream = new MemoryStream();
            await obj.Content.CopyToAsync(memoryStream);
            media.Content = memoryStream.ToArray().Decrypt();
        }

        return media;
    }

    public async IAsyncEnumerable<Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras = false)
    {
        foreach (var id in ids)
        {
            yield return await Load(id, removeWebsiteExtras);
        }
    }

    public async Task<Guid> Save(string content, Guid? id, string? extension = null, string? owner = null)
    {
        var mediaId = id ?? Guid.NewGuid();

        var metadata = new Dictionary<string, string>
        {
            [JsFormatMetaKey] = "true",
            [ExtensionMetaKey] = extension ?? "(unknown)",
            [OwnerMetaKey] = owner ?? string.Empty
        };

        var bytes = Encoding.UTF8.GetBytes(content);
        await _client.SaveAsync(KeyFor(mediaId), bytes.Encrypt(), "text/plain; charset=utf-8", metadata);

        return mediaId;
    }

    public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf", string? owner = null)
    {
        var mediaId = id ?? Guid.NewGuid();

        var metadata = new Dictionary<string, string>
        {
            [JsFormatMetaKey] = "false",
            [ExtensionMetaKey] = extension,
            [OwnerMetaKey] = owner ?? string.Empty
        };

        await _client.SaveAsync(KeyFor(mediaId), content.Encrypt(), GetContentType(extension), metadata);

        return mediaId;
    }

    /// <summary>
    ///     Placeholder hook for stripping any website-specific wrapping from stored string content
    ///     (e.g. embed boilerplate) before returning it to the caller. Adjust to match your actual format.
    /// </summary>
    private static string StripWebsiteExtras(string content) => content;

    private static string GetContentType(string extension) => extension.TrimStart('.').ToLowerInvariant() switch
    {
        "pdf" => "application/pdf",
        "png" => "image/png",
        "jpg" or "jpeg" => "image/jpeg",
        "gif" => "image/gif",
        "svg" => "image/svg+xml",
        "json" => "application/json",
        "txt" => "text/plain",
        _ => "application/octet-stream"
    };
}