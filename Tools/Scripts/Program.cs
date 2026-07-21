using Drive.API.External.Cloudflare;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Diagnostics;

// ---------------------------------------------------------------------------
// Migration: copies every Metadata/Blob row from SQL Server into Cloudflare R2.
// Assumes Metadata has a BlobId FK pointing at Blob.Id (adjust the JOIN below
// if your schema links them differently, e.g. shared PK).
// ---------------------------------------------------------------------------

string connectionString = "Server=(localdb)\\mssqllocaldb;Database=AppContext-drive;Trusted_Connection=True;MultipleActiveResultSets=true";

var cloudflareOptions = Options.Create(new CloudflareOptions
{
    AccountId = "",
    AccessKey = "",
    SecretKey = "",
    BucketName = "ls-api"
});

using var r2 = new CloudflareClient(cloudflareOptions);

const string query = """
    SELECT m.Id, m.Extension, m.Size, m.JsFormat, b.Content, b.ContentStr
    FROM Metadata m
    JOIN Container b ON b.Id = m.BlobId
    """;

var stopwatch = Stopwatch.StartNew();
int migrated = 0, skipped = 0, failed = 0;

// Cap concurrent uploads so we don't hammer R2 or the SQL connection.
using var throttle = new SemaphoreSlim(8);
var inFlight = new List<Task>();

await using var connection = new SqlConnection(connectionString);
await connection.OpenAsync();

await using var command = new SqlCommand(query, connection)
{
    CommandTimeout = 0 // long-running read; adjust if you prefer a timeout
};

await using var reader = await command.ExecuteReaderAsync();

while (await reader.ReadAsync())
{
    var id = reader.GetGuid(reader.GetOrdinal("Id"));
    var extension = reader.IsDBNull(reader.GetOrdinal("Extension"))
        ? null
        : reader.GetString(reader.GetOrdinal("Extension"));
    var jsFormat = reader.GetBoolean(reader.GetOrdinal("JsFormat"));

    byte[]? content = null;
    string? contentStr = null;

    var contentOrdinal = reader.GetOrdinal("Content");
    var contentStrOrdinal = reader.GetOrdinal("ContentStr");

    if (!reader.IsDBNull(contentOrdinal))
    {
        content = (byte[])reader.GetValue(contentOrdinal);
    }

    if (!reader.IsDBNull(contentStrOrdinal))
    {
        contentStr = reader.GetString(contentStrOrdinal);
    }

    await throttle.WaitAsync();

    var task = UploadOneAsync(id, extension, jsFormat, content, contentStr)
        .ContinueWith(t =>
        {
            throttle.Release();
            if (t.IsFaulted)
            {
                Interlocked.Increment(ref failed);
                Console.WriteLine($"[FAIL] {id}: {t.Exception?.GetBaseException().Message}");
            }
            else
            {
                Interlocked.Increment(ref migrated);
            }
        });

    inFlight.Add(task);

    // Periodically drain finished tasks so the list doesn't grow unbounded.
    if (inFlight.Count >= 200)
    {
        await Task.WhenAll(inFlight);
        inFlight.Clear();
    }
}

await Task.WhenAll(inFlight);

stopwatch.Stop();
Console.WriteLine();
Console.WriteLine($"Done in {stopwatch.Elapsed}. Migrated: {migrated}, Skipped: {skipped}, Failed: {failed}");

async Task UploadOneAsync(Guid id, string? extension, bool jsFormat, byte[]? content, string? contentStr)
{
    var key = id.ToString("N");

    jsFormat = jsFormat || !string.IsNullOrEmpty(contentStr);

    var metadata = new Dictionary<string, string>
    {
        ["jsformat"] = jsFormat ? "true" : "false",
        ["extension"] = extension ?? "(unknown)"
    };

    if (jsFormat)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(contentStr!);
        await r2.SaveAsync(key, bytes, "text/plain; charset=utf-8", metadata);
    }
    else
    {
        var contentType = GetContentType(extension);
        await r2.SaveAsync(key, content!, contentType, metadata);
    }

    Console.WriteLine($"[OK]   {id} ({(jsFormat ? "string" : "binary")}, ext={extension ?? "?"})");
}

static string GetContentType(string? extension) => extension?.TrimStart('.').ToLowerInvariant() switch
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