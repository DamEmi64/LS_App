using Base;
using Communication.Infrastructure.Services.SendService.Models;

namespace Communication.Infrastructure.Jobs
{
    public class GenEmailJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public string Name => $"Generate email {Model?.Template?.Subject} for {string.Join(",", Model?.Recipients.Select(x => x.Email) ?? Array.Empty<string>())}";

        public EmailGenerationModel? Model { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateFromTemplate;
    }
}