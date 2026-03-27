using Base;
using Files.Infrastructure.Jobs;

namespace Files.Infrastructure.Services.ManagmentService
{
    public class ManagmentService : IManagmentService
    {
        private readonly IJobEngine _jobEngine;

        public ManagmentService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task<string> CopyFile(Domain.Entities.File file, string destination, UserData userData)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var location = file.Locaction ?? throw new ArgumentNullException($"{nameof(file)}.{nameof(file.Locaction)}");

            var title = $"Copy files from {location} to {destination}";

            var schema = _jobEngine.Create(title)
                .AddJob(new CopyFileJob
                {
                    Source = location,
                    Destination = destination
                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }

        public async Task<string> DeleteFile(Domain.Entities.File file, UserData userData)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var location = file.Locaction ?? throw new ArgumentNullException($"{nameof(file)}.{nameof(file.Locaction)}");

            var title = $"Delete files from {location}";

            var schema = _jobEngine.Create(title)
                .AddJob(new DeleteFileJob
                {
                    Locaction = location
                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }

        public async Task<string> MoveFile(Domain.Entities.File file, string destination, UserData userData)
        {
            if (file is null)
                throw new ArgumentNullException(nameof(file));

            var location = file.Locaction ?? throw new ArgumentNullException($"{nameof(file)}.{nameof(file.Locaction)}");

            var title = $"Move files from {location} to {destination}";

            var schema = _jobEngine.Create(title)
                .AddJob(new MoveFileJob
                {
                    Source = location,
                    Destination = destination
                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }
    }
}