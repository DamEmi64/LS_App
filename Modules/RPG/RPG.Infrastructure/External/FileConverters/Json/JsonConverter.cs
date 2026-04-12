using RPG.Domain.Entities;

namespace RPG.Infrastructure.External.FileConverters.Json
{
    public class JsonConverter : IFileConverter
    {
        public FileConverterType Type => FileConverterType.Json;

        Task<Story> IFileConverter.Convert(string data)
        {
            throw new NotImplementedException();
        }

        Task<string> IFileConverter.Convert(Story story)
        {
            throw new NotImplementedException();
        }
    }
}
