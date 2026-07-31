using Base;
using FilesV2.Domain.Enums;

namespace FilesV2.Domain.Entities
{
    public class FileUser : Entity
    {
        public required string UserId { get; set; }
        public required string Login { get; set; }
        public Privilage Privilage { get; set; }
    }
}
