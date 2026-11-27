using Base;
using Communication.Domain.Repositories;
using System.ComponentModel;

namespace Communication.Infrastructure.Workers
{
    public class CommunicationHistoryCleanWorker : IWorker
    {
        private readonly ICommunicationHistoryRepository _communicationHistoryRepository;


        public CommunicationHistoryCleanWorker(ICommunicationHistoryRepository communicationHistoryRepository)
        {
            _communicationHistoryRepository = communicationHistoryRepository;
        }

        public string Cron => "0 0 1 * *"; // Every 1st of month at 00:00

        [DisplayName("[WORKER] Communication History Cleaner")]
        public void Execute()
        {
            var today = DateTime.UtcNow;
            var logsToDelete = _communicationHistoryRepository.GetCommunicationHistoryByDateRange(today.AddMonths(-1), today);
            _communicationHistoryRepository.DeleteLogs(logsToDelete);
        }
    }
}
