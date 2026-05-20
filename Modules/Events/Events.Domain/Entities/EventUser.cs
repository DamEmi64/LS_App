using Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Domain.Entities
{
    public class EventUser
    {
        public Guid Id { get; set; }

        public required string UserId { get; set; }

        public string? Login { get; set; }

        public string? Email { get; set; }

        public bool Invited { get; set;  }

        [JsonIgnore]
        public required Event Event { get; set; }
    }
}
