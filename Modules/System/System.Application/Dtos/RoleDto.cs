namespace System.Application.Dtos
{
    public class RoleDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public List<ClaimDto> Claims { get; set; } = new();
    }

    public class ClaimDto
    {
        public required string Key { get; set; }
        public required string Value { get; set; }
        public required string Description { get; set; }
    }
}