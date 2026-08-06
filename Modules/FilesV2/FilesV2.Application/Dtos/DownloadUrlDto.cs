namespace FilesV2.Application.Dtos
{
    public class DownloadUrlDto
    {
        public string DownloadUrl { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
