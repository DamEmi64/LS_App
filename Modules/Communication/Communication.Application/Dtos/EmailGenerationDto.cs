using Base;
using Communication.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Communication.Application.Dtos
{
    public class EmailGenerationDto
    {
        public Guid? Template { get; set; }
        public UserData? Sender { get; set; }
        public IEnumerable<UserData> Recipients { get; set; } = Array.Empty<UserData>();
    }
}
