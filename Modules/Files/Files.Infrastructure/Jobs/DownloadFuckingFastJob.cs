using Base;
using Files.Domain.Entities;
using Files.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;

namespace Files.Infrastructure.Jobs
{
    public class DownloadFuckingFastJob : IJob
    {
        private readonly Regex regex = new(@"(window\.open\(\"").*(\"")");
        private readonly Regex regexName = new(@"(<span class=\""text-xl\"">)(.*)(<\/span>)");

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Download file from {Link.Link} to {Locaction}";

        public required string Locaction { get; set; }

        public required SourceLink Link { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.ImportFile;

        public async Task Execute(IJobContext jobContext)
        {
            var httpFactory = jobContext.ServiceProvider.GetRequiredService<IHttpClientFactory>();
            var repo = jobContext.ServiceProvider.GetRequiredService<IFileRepository>();

            await ExecuteInternal(httpFactory, jobContext);
            await repo.CheckLink(Link.Id);
        }

        private async Task ExecuteInternal(IHttpClientFactory httpClientFactory, IJobContext jobContext)
        {
            try
            {
                using (var httpClient = httpClientFactory.CreateClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                    var response = await httpClient.GetAsync(Link.Link);
                    var httpContent = await response.Content.ReadAsStringAsync();

                    if (httpContent == "rate limited")
                    {
                        throw new Exception("Rate limited!");
                    }

                    var fileNameRegex = regexName.Match(httpContent);

                    if (!fileNameRegex.Success)
                    {
                        throw new Exception("No file name");
                    }

                    var filename = fileNameRegex.Value.Replace(@"<span class=""text-xl"">", "").Replace(@"</span>", "");
                    foreach (Match myMatch in regex.Matches(httpContent))
                    {
                        if (myMatch.Success)
                        {
                            var downloadLink = myMatch.Value.Replace(@"window.open(""", "").Replace(@"""", "");
                            await jobContext.AddLog($"Link founded. Downloading file from: {Link.Link}");
                            HttpResponseMessage download;

                            download = await httpClient.GetAsync($"{downloadLink}");
                            await jobContext.AddLog("Download complete. Saving...");

                            if (!Directory.Exists(Locaction))
                            {
                                Directory.CreateDirectory(Locaction);
                            }

                            await System.IO.File.WriteAllBytesAsync($"{Locaction}\\{filename}", await download.Content.ReadAsByteArrayAsync());
                            await jobContext.AddLog("File saved");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await jobContext.AddError(ex.Message);
                throw;
            }
        }
    }
}