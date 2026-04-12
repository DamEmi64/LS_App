using RPG.Domain.Entities;
namespace RPG.Infrastructure.External.FileConverters
{
    public interface IFileConverter
    {
        public FileConverterType Type { get; }
        Task<Story> Convert(string data);
        Task<string> Convert(Story story);
    }

    public enum FileConverterType
    {
        Json,
        OldJson
    }
}
