using Base;
using Files.Domain.Dictionaries;
using Files.Domain.Entities;
using Files.Infrastructure.Jobs;

namespace Files.Infrastructure.Services.DownloadService
{
    public class ImportService : IImportService
    {
        private readonly IJobEngine _jobEngine;

        public ImportService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task<string> ImportFile(Domain.Entities.File file, UserData userData)
        {
            var title = $"Import file/files for {file.Title}";

            var schema = _jobEngine.Create(title);

            var linkGroups = file.Sources.Select((x, idx) => new { Links = x, idx })
                            .GroupBy(x => x.idx / 25 + 1);

            foreach (var linkGroup in linkGroups)
            {
                var links = linkGroup.Select(x => x.Links).ToList();

                var firstLink = links.LastOrDefault();

                IProcessJobSchema? groupSchema = true switch
                {
                    true when firstLink?.SourceType == SourceTypes.Local => schema.AddJob(await AddImportFromLocalJob(firstLink, file.Locaction ?? throw new NullReferenceException(), userData)),
                    true when firstLink?.SourceType == SourceTypes.FuckingFast => schema.AddJob(await AddDownloadFromFuckingFastJob(firstLink, file.Locaction ?? throw new NullReferenceException(), userData)),
                    _ => null
                };

                if (groupSchema is null || firstLink is null)
                {
                    continue;
                }

                links.Remove(firstLink);

                foreach (var link in links)
                {
                    groupSchema = true switch
                    {
                        true when link?.SourceType == SourceTypes.Local => groupSchema.AddChildJob(await AddImportFromLocalJob(link, file.Locaction ?? throw new NullReferenceException(), userData)),
                        true when link?.SourceType == SourceTypes.FuckingFast => groupSchema.AddChildJob(await AddDownloadFromFuckingFastJob(link, file.Locaction ?? throw new NullReferenceException(), userData)),
                        _ => groupSchema
                    };
                }
            }

            await _jobEngine.Execute(schema, userData);

            return title;
        }

        private async Task<IJob> AddDownloadFromFuckingFastJob(SourceLink link, string locaction, UserData userData)
        {
            return new DownloadFuckingFastJob
            {
                Link = link,
                Locaction = locaction,
            };
        }

        private async Task<IJob> AddImportFromLocalJob(SourceLink link, string locaction, UserData userData)
        {
            return new CopyFileJob
            {
                Source = link.Link,
                Destination = locaction
            };
        }
    }
}