using Base;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Services.SummaryService
{
    public interface ISummaryService
    {
        Task<string> QueueGenerateSummaryJob(Guid id, SummaryModel summary, UserData userData, bool pdf);

        Task<string> QueueSendToFirebaseJob(Guid id, SummaryModel summary, UserData userData);
    }
}