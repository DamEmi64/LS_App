using Base;

namespace FilesV2.Domain.Entities
{
    public abstract class PermittedEntity : Entity
    {
        public required CatalogUser Owner { get; set; }
        public List<CatalogUser> Users { get; set; } = new();
        public bool Public { get; set; } = false;
    }
}
