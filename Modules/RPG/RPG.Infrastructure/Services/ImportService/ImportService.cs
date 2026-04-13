using Base;
using Microsoft.AspNetCore.Http;
using RPG.Infrastructure.Jobs;
using RPG.Infrastructure.Models;
using System.Text;

namespace RPG.Infrastructure.Services
{
    public class ImportService : IImportService
    {
        private readonly IJobEngine _jobEngine;

        public ImportService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task<string> ImportFromFile(IFormFile? file, int converterType, string? externalUrl, UserData user)
        {
            var rpgTitle = file?.FileName ?? externalUrl ?? "Unknown RPG";
            var title = $"Import RPG from file {rpgTitle}";
            string? content = null;

            if (file is not null)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    content = Encoding.UTF8.GetString(stream.ToArray());
                }
            }

            var model = new ImportRPGModel
            {
                Title = Path.GetFileNameWithoutExtension(rpgTitle),
                Type = converterType,
                ExternalUrl = externalUrl,
                FileContent = content
            };

            var schema = _jobEngine.Create(title)
                .AddJob(new ImportRPGFromFileJob
                {
                    Model = model
                });

            await _jobEngine.Execute(schema, user);

            return title;
        }
    }
}
