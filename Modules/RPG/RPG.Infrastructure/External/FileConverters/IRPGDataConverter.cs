using RPG.Domain.Entities;
namespace RPG.Infrastructure.External.FileConverters
{
    public interface IRPGDataConverter
    {
        public int Type { get; }
        Task<Story> Convert(string data);
        Task<string> Convert(Story story);
    }

}
