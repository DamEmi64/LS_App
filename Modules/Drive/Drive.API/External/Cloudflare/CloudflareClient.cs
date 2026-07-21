using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.Internal;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using System.Text;

namespace Drive.API.External.Cloudflare;

public class CloudflareClient : IDisposable
{
    private readonly IAmazonS3 _client;
    private readonly string _bucketName;

    public CloudflareClient(IOptions<CloudflareOptions> options)
    {
        AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;
        AWSConfigs.LoggingConfig.LogResponses = ResponseLoggingOption.Always;
        AWSConfigs.LoggingConfig.LogMetrics = true;
        AWSConfigsS3.DisableDefaultChecksumValidation = true;

        var settings = options.Value;
        _bucketName = settings.BucketName;

        var config = new AmazonS3Config
        {
            ServiceURL = $"https://{settings.AccountId}.r2.cloudflarestorage.com",
            ForcePathStyle = true,

            AuthenticationRegion = "auto",

            RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED,
            ResponseChecksumValidation = ResponseChecksumValidation.WHEN_REQUIRED
        };

        _client = new AmazonS3Client(
            settings.AccessKey,
            settings.SecretKey,
            config);
    }

    /// <summary>
    /// Uploads a stream to R2 under the given key, with optional custom metadata (stored as x-amz-meta-* headers).
    /// </summary>
    public async Task SaveAsync(
        string key,
        Stream content,
        string? contentType = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = content,
            ContentType = contentType ?? "application/octet-stream",
            AutoCloseStream = false,
            DisablePayloadSigning = true,
            DisableDefaultChecksumValidation = true
        };

        if (metadata != null)
        {
            foreach (var (metaKey, metaValue) in metadata)
            {
                request.Metadata.Add(metaKey, metaValue);
            }
        }

        await _client.PutObjectAsync(request, cancellationToken);
    }

    /// <summary>
    /// Uploads a byte array to R2 under the given key, with optional custom metadata.
    /// </summary>
    public async Task SaveAsync(
        string key,
        byte[] content,
        string? contentType = null,
        IDictionary<string, string>? metadata = null,
        CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream(content);
        await SaveAsync(key, stream, contentType, metadata, cancellationToken);
    }

    /// <summary>
    /// Downloads an object from R2 into a memory stream. Caller owns and disposes the returned stream.
    /// </summary>
    public async Task<Stream> GetAsync(string key, CancellationToken cancellationToken = default)
    {
        using var full = await GetWithMetadataAsync(key, cancellationToken);
        var copy = new MemoryStream();
        await full.Content.CopyToAsync(copy, cancellationToken);
        copy.Position = 0;
        return copy;
    }

    /// <summary>
    /// Downloads an object along with its custom metadata (e.g. extension, owner, format flags).
    /// Caller must dispose the returned <see cref="R2Object"/>.
    /// </summary>
    public async Task<R2Object> GetWithMetadataAsync(string key, CancellationToken cancellationToken = default)
    {
        var request = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        var response = await _client.GetObjectAsync(request, cancellationToken);

        var memoryStream = new MemoryStream();
        await response.ResponseStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;

        // The SDK exposes custom metadata keys without the "x-amz-meta-" prefix.
        var metadata = response.Metadata.Keys
            .ToDictionary(k => k, k => response.Metadata[k]);
        var contentType = response.Headers.ContentType;

        response.Dispose();

        return new R2Object
        {
            Content = memoryStream,
            ContentType = contentType,
            Metadata = metadata
        };
    }

    /// <summary>
    /// Checks whether an object exists at the given key.
    /// </summary>
    public async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.GetObjectMetadataAsync(_bucketName, key, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Deletes an object at the given key. Does not throw if the key doesn't exist.
    /// </summary>
    public async Task DeleteAsync(string key, CancellationToken cancellationToken = default)
    {
        await _client.DeleteObjectAsync(_bucketName, key, cancellationToken);
    }

    /// <summary>
    /// Generates a pre-signed URL for temporary access (upload or download) without exposing credentials.
    /// </summary>
    public string GetPresignedUrl(
        string key,
        TimeSpan expiresIn,
        HttpVerb verb = HttpVerb.GET)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = verb,
            Expires = DateTime.UtcNow.Add(expiresIn)
        };

        return _client.GetPreSignedURL(request);
    }

    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}