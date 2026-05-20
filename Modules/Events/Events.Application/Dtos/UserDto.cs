using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Application.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string? UserId { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }
    }
}
