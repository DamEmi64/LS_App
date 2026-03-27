using Base;
using Communication.Domain.Entities;

namespace Communication.Infrastructure.Services.SendService.Models
{
    public class EmailGenerationModel
    {
        public Template? Template { get; set; }
        public UserData? Sender { get; set; }
        public IEnumerable<UserData> Recipients { get; set; } = Array.Empty<UserData>();
    }
}