using FilesV2.Domain.Enums;

namespace FilesV2.Application.Dtos
{
    public class GrantAccessDto
    {
        public required string UserId { get; set; }
        public required string Login { get; set; }
        public Privilage Privilage { get; set; }
    }
}
