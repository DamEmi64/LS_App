using System;
using System.Collections.Generic;
using System.Text;

namespace RPG.Application.Dtos
{
    public class RpgFileDto
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public Guid Content { get; set; }
    }
}
