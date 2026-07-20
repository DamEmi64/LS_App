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

const string selectSql = @"
SELECT
    Id,
    Content,
    ContentStr,
    Extension,
    InsDate,
    UpdDate,
    InsBy,
    UpdBy
FROM Media_old
ORDER BY Id;";

await using var selectCommand = new SqlCommand(selectSql, source);

await using var reader = await selectCommand.ExecuteReaderAsync();

const string insertContainerSql = @"
INSERT INTO Container
(
    Id,
    Content,
    ContentStr,
    InsDate,
    UpdDate,
    InsBy,
    UpdBy
)
VALUES
(
    @Id,
    @Content,
    @ContentStr,
    @InsDate,
    @UpdDate,
    @InsBy,
    @UpdBy
);";

const string insertMetadataSql = @"
INSERT INTO Metadata
(
    Id,
    Extension,
    Size,
    JsFormat,
    BlobId,
    InsDate,
    UpdDate,
    InsBy,
    UpdBy
)
VALUES
(
    @Id,
    @Extension,
    @Size,
    @JsFormat,
    @BlobId,
    @InsDate,
    @UpdDate,
    @InsBy,
    @UpdBy
);";

await using var insertContainer = new SqlCommand(insertContainerSql, destination, (SqlTransaction)transaction);
await using var insertMetadata = new SqlCommand(insertMetadataSql, destination, (SqlTransaction)transaction);

#region Container parameters

insertContainer.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
insertContainer.Parameters.Add("@Content", SqlDbType.VarBinary, -1);
insertContainer.Parameters.Add("@ContentStr", SqlDbType.NVarChar, -1);
insertContainer.Parameters.Add("@InsDate", SqlDbType.DateTimeOffset);
insertContainer.Parameters.Add("@UpdDate", SqlDbType.DateTimeOffset);
insertContainer.Parameters.Add("@InsBy", SqlDbType.NVarChar, -1);
insertContainer.Parameters.Add("@UpdBy", SqlDbType.NVarChar, -1);

#endregion

#region Metadata parameters

insertMetadata.Parameters.Add("@Id", SqlDbType.UniqueIdentifier);
insertMetadata.Parameters.Add("@Extension", SqlDbType.NVarChar, -1);
insertMetadata.Parameters.Add("@Size", SqlDbType.Int);
insertMetadata.Parameters.Add("@JsFormat", SqlDbType.Bit);
insertMetadata.Parameters.Add("@BlobId", SqlDbType.UniqueIdentifier);
insertMetadata.Parameters.Add("@InsDate", SqlDbType.DateTimeOffset);
insertMetadata.Parameters.Add("@UpdDate", SqlDbType.DateTimeOffset);
insertMetadata.Parameters.Add("@InsBy", SqlDbType.NVarChar, -1);
insertMetadata.Parameters.Add("@UpdBy", SqlDbType.NVarChar, -1);

#endregion

try
{
    while (await reader.ReadAsync())
    {
        var id = reader.GetGuid(0);

        byte[]? content = reader.IsDBNull(1)
            ? null
            : (byte[])reader["Content"];

        string? contentStr = reader.IsDBNull(2)
            ? null
            : reader.GetString(2);

        var extension = reader.GetString(3);

        var insDate = reader.GetFieldValue<DateTimeOffset>(4);
        var updDate = reader.GetFieldValue<DateTimeOffset>(5);

        string? insBy = reader.IsDBNull(6) ? null : reader.GetString(6);
        string? updBy = reader.IsDBNull(7) ? null : reader.GetString(7);

        var originalSize = content?.Length ?? 0;

        content = content?.Let(EncryptBytes);
        contentStr = contentStr?.Let(EncryptString);

        // Container

        insertContainer.Parameters["@Id"].Value = id;
        insertContainer.Parameters["@Content"].Value = (object?)content ?? DBNull.Value;
        insertContainer.Parameters["@ContentStr"].Value = (object?)contentStr ?? DBNull.Value;
        insertContainer.Parameters["@InsDate"].Value = insDate;
        insertContainer.Parameters["@UpdDate"].Value = updDate;
        insertContainer.Parameters["@InsBy"].Value = (object?)insBy ?? DBNull.Value;
        insertContainer.Parameters["@UpdBy"].Value = (object?)updBy ?? DBNull.Value;

        await insertContainer.ExecuteNonQueryAsync();

        // Metadata

        insertMetadata.Parameters["@Id"].Value = id;
        insertMetadata.Parameters["@Extension"].Value = extension;
        insertMetadata.Parameters["@Size"].Value = originalSize;
        insertMetadata.Parameters["@JsFormat"].Value = false;
        insertMetadata.Parameters["@BlobId"].Value = id;
        insertMetadata.Parameters["@InsDate"].Value = insDate;
        insertMetadata.Parameters["@UpdDate"].Value = updDate;
        insertMetadata.Parameters["@InsBy"].Value = (object?)insBy ?? DBNull.Value;
        insertMetadata.Parameters["@UpdBy"].Value = (object?)updBy ?? DBNull.Value;

        await insertMetadata.ExecuteNonQueryAsync();

        Console.WriteLine(id);
    }

    await transaction.CommitAsync();

    Console.WriteLine("Migration completed.");
}
catch
{
    await transaction.RollbackAsync();
    throw;
}

byte[] EncryptBytes(byte[] data)
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
        crypto.Write(data);
        crypto.FlushFinalBlock();
    }

    return output.ToArray();
}

string EncryptString(string text)
{
    return Convert.ToBase64String(
        EncryptBytes(Encoding.UTF8.GetBytes(text)));
}

static class Extensions
{
    public static TResult Let<T, TResult>(this T value, Func<T, TResult> func)
        => func(value);
}