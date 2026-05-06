using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace RPG.Application.Dtos
{
    public class CreateStoryDto
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public List<IFormFile> Files { get; set; } = new List<IFormFile>();
    }
}
