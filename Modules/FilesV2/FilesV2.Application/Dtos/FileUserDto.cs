using FilesV2.Domain.Enums;

namespace FilesV2.Application.Dtos
{
    public class FileUserDto
    {
        public required string UserId { get; set; }
        public string Login { get; set; } = string.Empty;
        public Privilage Privilage { get; set; }
    }
}
