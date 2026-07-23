namespace FilesV2.Application.Dtos
{
    public class UploadUrlDto
    {
        public UploadUrlDto()
        {
        }

        public Guid Content { get; set; }
        public string UploadUrl { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
