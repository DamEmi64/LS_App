namespace RPG.Application.Dtos
{
    public class LinkDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Url { get; set; }
    }
}