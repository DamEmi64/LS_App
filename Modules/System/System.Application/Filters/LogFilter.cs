using Microsoft.AspNetCore.Mvc;

namespace System.Application.Filters
{
    public class LogFilter
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        [FromQuery(Name = "search[value]")]
        public string? SearchValue { get; set; }
        public string? Level { get; set; }
        public string? Method { get; set; }
    }
}