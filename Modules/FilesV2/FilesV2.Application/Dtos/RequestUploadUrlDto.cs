namespace FilesV2.Application.Dtos
{
    public class RequestUploadUrlDto
    {
        public string MimeType { get; set; } = string.Empty;
        public long SizeBytes { get; set; }
    }
}
