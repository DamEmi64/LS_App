namespace CommunicationBase.Dtos
{
    public class DiscordResponse
    {
        public string Text { get; set; } = string.Empty;
        public List<DiscordResponseFile>? Files { get; set; }

        public class DiscordResponseFile
        {
            public required string Title { get; set; }
            public required byte[] Content { get; set; }
            public required string Extension { get; set; }
            public string Filename => $"{Title}.{Extension}";
        }
    }
}
