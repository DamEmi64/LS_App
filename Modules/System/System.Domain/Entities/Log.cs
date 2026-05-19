namespace System.Domain.Entities
{
    public class Log
    {
        public int Id { get; set; }
        public required string Message { get; set; }
        public required string Level { get; set; }
        public string? Exception { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? HttpMethod { get; set; }
        public string? HttpUri { get; set; }
        public string? User { get; set; }
    }
}