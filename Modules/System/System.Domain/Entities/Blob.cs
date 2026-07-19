using Base;
namespace System.Domain.Entities
{
    public class Blob : Entity
    {
        public byte[]? Content { get; set; }
        public string? ContentStr { get; set; }
    }
}
