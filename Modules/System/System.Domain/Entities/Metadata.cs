using Base;

namespace System.Domain.Entities
{
    public class Metadata : Entity
    {
        public string? Extension { get; set; }
        public int Size { get; set; }
        public bool JsFormat { get; set; }
        public required Blob Blob { get; set; }
    }
}
