namespace Translations.Dtos
{
    public class DictionaryDto
    {
        public int Key { get; set; }
        public string Dictionary { get; set; } = string.Empty;
        public string? TitlePL { get; set; }
        public string? DescriptionPL { get; set; }
        public string? TitleDE { get; set; }
        public string? DescriptionDE { get; set; }
        public string? TitleFR { get; set; }
        public string? DescriptionFR { get; set; }
        public string? TitleEN { get; set; }
        public string? DescriptionEN { get; set; }
    }
}
