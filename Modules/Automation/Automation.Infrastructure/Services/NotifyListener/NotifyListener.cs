using Automation.Domain.Repositories;
using Automation.Infrastructure.Services.AutomationService;
using Base;

namespace Automation.Infrastructure.Services.NotifyListener
{
    public class NotifyListener : INotifierInstance
    {
        private readonly IAutomationService _automationService;
        private readonly IAutomatRepository _automatRepository;

        public NotifyListener(IAutomatRepository automatRepository, IAutomationService automationService)
        {
            _automatRepository = automatRepository;
            _automationService = automationService;
        }

        public Task Error(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        public Task Info(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        public Task Process(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        public Task ProcessError(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        public Task Success(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        public Task Warning(int messageId, params object[] args)
        {
            return CheckAutomats(messageId);
        }

        private async Task CheckAutomats(int messageId)
        {
            var automats = _automatRepository.TriggeredByEvent(messageId);

            foreach (var automat in automats)
            {
                await _automationService.ExecuteAutomatAsync(automat);
                automat.LastRun = DateTimeOffset.UtcNow;
                await _automatRepository.Update(automat);
            }
        }
    }
}
