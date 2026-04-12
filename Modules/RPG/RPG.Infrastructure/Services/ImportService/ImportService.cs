using Base;
using Microsoft.AspNetCore.Http;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.Jobs;
using System;
using System.Collections.Generic;
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

        public async Task<string> ImportFromFile(IFormFile file, FileConverterType converterType, UserData user)
        {
            var title = $"Import RPG from file {file.FileName}";
            string? content = null;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                content = Encoding.UTF8.GetString(stream.ToArray());
            }

            var schema = _jobEngine.Create(title)
                .AddJob(new ImportRPGFromFileJob
                {
                    FileContent = content,
                    RPGName = file.FileName,
                    ConverterType = converterType
                });

            await _jobEngine.Execute(schema, user);

            return title;
        }
    }
}
