using Base;
using Files.Domain.Repositories;
using System.Text.RegularExpressions;

namespace Files.Infrastructure.Jobs.DownloadFuckingFast
{
    public class DownloadFuckingFastJobHandler : JobHandler<DownloadFuckingFastJob>
    {
        private readonly Regex regex = new(@"(window\.open\(\"").*(\"")");
        private readonly Regex regexName = new(@"(<span class=\""text-xl\"">)(.*)(<\/span>)");

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFileRepository _fileRepository;

        public DownloadFuckingFastJobHandler(IJobContext jobContext,
            IHttpClientFactory httpClientFactory,
            IFileRepository fileRepository)
            : base(jobContext)
        {
            _httpClientFactory = httpClientFactory;
            _fileRepository = fileRepository;
        }

        public override async Task Execute(DownloadFuckingFastJob request)
        {
            try
            {
                using (var httpClient = _httpClientFactory.CreateClient())
                {
                    httpClient.Timeout = TimeSpan.FromMinutes(5);

                    var response = await httpClient.GetAsync(request.Link.Link);
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
                            await Log($"Link founded. Downloading file from: {request.Link.Link}");
                            HttpResponseMessage download;

                            download = await httpClient.GetAsync($"{downloadLink}");
                            await Log("Download complete. Saving...");
                            if (!Directory.Exists(request.Locaction))
                            {
                                Directory.CreateDirectory(request.Locaction);
                            }

                            await System.IO.File.WriteAllBytesAsync($"{request.Locaction}\\{filename}", await download.Content.ReadAsByteArrayAsync());
                            await Log("File saved");

                            await _fileRepository.CheckLink(request.Link.Id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await LogError(ex.Message);
            }
        }
    }
}
