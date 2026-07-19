using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using System.Text;

const string sourceConnectionString = "...";
const string destinationConnectionString = "...";

const string encryptionKey = "..."; // Base64 encoded 32-byte key
const string encryptionIV = "...";  // Base64 encoded 16-byte IV

var key = Convert.FromBase64String(encryptionKey);
var iv = Convert.FromBase64String(encryptionIV);

await using var source = new SqlConnection(sourceConnectionString);
await using var destination = new SqlConnection(destinationConnectionString);

await source.OpenAsync();
await destination.OpenAsync();

await using var transaction = await destination.BeginTransactionAsync();

try
{
    const string selectSql = @"
SELECT
    m.Id,
    m.Extension,
    m.Size,
    m.JsFormat,
    b.Id AS BlobId,
    b.Content,
    b.ContentStr
FROM Metadata m
INNER JOIN Blob b ON b.Id = m.BlobId
ORDER BY m.Id;";

    await using var selectCommand = new SqlCommand(selectSql, source);
    await using var reader = await selectCommand.ExecuteReaderAsync();

    while (await reader.ReadAsync())
    {
        var metadataId = reader.GetGuid(0);
        var extension = reader.IsDBNull(1) ? null : reader.GetString(1);
        var size = reader.GetInt32(2);
        var jsFormat = reader.GetBoolean(3);

        var blobId = reader.GetGuid(4);

        byte[]? content = reader.IsDBNull(5)
            ? null
            : (byte[])reader["Content"];

        string? contentStr = reader.IsDBNull(6)
            ? null
            : reader.GetString(6);

        if (content != null)
            content = Encrypt(content);

        if (contentStr != null)
            contentStr = EncryptStr(contentStr);

        const string insertBlobSql = @"
INSERT INTO Blob
(
    Id,
    Content,
    ContentStr
)
VALUES
(
    @Id,
    @Content,
    @ContentStr
);";

        await using (var blobCommand = new SqlCommand(insertBlobSql, destination, (SqlTransaction)transaction))
        {
            blobCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = blobId;

            blobCommand.Parameters.Add("@Content", SqlDbType.VarBinary, -1)
                .Value = (object?)content ?? DBNull.Value;

            blobCommand.Parameters.Add("@ContentStr", SqlDbType.NVarChar, -1)
                .Value = (object?)contentStr ?? DBNull.Value;

            await blobCommand.ExecuteNonQueryAsync();
        }

        const string insertMetadataSql = @"
INSERT INTO Metadata
(
    Id,
    Extension,
    Size,
    JsFormat,
    BlobId,
    Encrypted
)
VALUES
(
    @Id,
    @Extension,
    @Size,
    @JsFormat,
    @BlobId,
    1
);";

        await using (var metadataCommand = new SqlCommand(insertMetadataSql, destination, (SqlTransaction)transaction))
        {
            metadataCommand.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = metadataId;

            metadataCommand.Parameters.Add("@Extension", SqlDbType.NVarChar, 50)
                .Value = (object?)extension ?? DBNull.Value;

            metadataCommand.Parameters.Add("@Size", SqlDbType.Int).Value = size;
            metadataCommand.Parameters.Add("@JsFormat", SqlDbType.Bit).Value = jsFormat;
            metadataCommand.Parameters.Add("@BlobId", SqlDbType.UniqueIdentifier).Value = blobId;

            await metadataCommand.ExecuteNonQueryAsync();
        }

        Console.WriteLine($"Migrated: {metadataId}");
    }

    await transaction.CommitAsync();

    Console.WriteLine("Migration completed successfully.");
}
catch (Exception ex)
{
    await transaction.RollbackAsync();

    Console.WriteLine(ex);

    throw;
}

byte[] Encrypt(byte[] data)
{
    using var aes = Aes.Create();

    aes.Key = key;
    aes.IV = iv;
    aes.Mode = CipherMode.CBC;
    aes.Padding = PaddingMode.PKCS7;

    using var encryptor = aes.CreateEncryptor();

    using var output = new MemoryStream();

    using (var crypto = new CryptoStream(output, encryptor, CryptoStreamMode.Write))
    {
        crypto.Write(data, 0, data.Length);
        crypto.FlushFinalBlock();
    }

    return output.ToArray();
}

string EncryptStr(string text)
{
    var bytes = Encoding.UTF8.GetBytes(text);
    return Convert.ToBase64String(Encrypt(bytes));
}