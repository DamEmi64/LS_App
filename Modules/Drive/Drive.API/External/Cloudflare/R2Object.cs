namespace Drive.API.External.Cloudflare;


/// <summary>
///     Result of a read that includes both the object content and its custom metadata.
/// </summary>
public sealed class R2Object : IDisposable
{
    public required Stream Content { get; init; }
    public string? ContentType { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();

    public void Dispose() => Content.Dispose();
}
