using Base;
using RPG.Infrastructure.Jobs;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Services.SummaryService
{
    public class SummaryService : ISummaryService
    {
        private readonly IJobEngine _jobEngine;

        public SummaryService(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }

        public async Task<string> QueueGenerateSummaryJob(Guid id, SummaryModel summary, UserData userData, bool pdf)
        {
            var title = $"Generate summary for {summary.Title}";
            var schema = _jobEngine.Create(title)
                .AddJob(new GenerateStoryFromSummaryJob
                {
                    Summary = summary,
                }).AddChildJob(new GenerateSummaryJob
                {
                    StoryId = id,
                    IsPdf = pdf
                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }

        public async Task<string> QueueSendToFirebaseJob(Guid id, SummaryModel summary, UserData userData)
        {
            var title = $"Send {summary.Title} to firebase";
            var schema = _jobEngine.Create(title)
                .AddJob(new GenerateStoryFromSummaryJob
                {
                    Summary = summary,
                }).AddChildJob(new SendToFirebaseJob
                {
                    StoryId = id,
                });

            await _jobEngine.Execute(schema, userData);

            return title;
        }
    }
}