using Base;
using RPG.Domain.Dictionaries;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class ImportRPGFromFileJob : IJob
    {
        public int OperationId => Operations.ImportRPGFromFile;

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public string Name => $"Get {Model?.Title ?? string.Empty} from file";

        public ImportRPGModel? Model { get; set; }
    }
}