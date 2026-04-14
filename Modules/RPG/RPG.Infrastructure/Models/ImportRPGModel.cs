namespace RPG.Infrastructure.Models
{
    public class ImportRPGModel
    {
        public string? FileContent { get; set; }

        public required string Title { get; set; }

        public string? ExternalUrl { get; set; }

        public int Type { get; set; }
    }
}
