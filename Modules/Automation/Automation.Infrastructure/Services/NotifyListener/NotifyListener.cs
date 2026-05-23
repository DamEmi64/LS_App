using Automation.Domain.Repositories;
using Automation.Infrastructure.Services.AutomationService;
using Base;
using Base.Automation;

namespace Automation.Infrastructure.Services.NotifyListener
{
    public class NotifyListener : INotifierInstance
    {
        private readonly IAutomationService _automationService;
        private readonly IAutomatRepository _automatRepository;
        private readonly List<IAutomationResolver> _resolvers;

        public NotifyListener(IAutomatRepository automatRepository, IAutomationService automationService, IEnumerable<IAutomationResolver> resolvers)
        {
            _automatRepository = automatRepository;
            _automationService = automationService;
            _resolvers = resolvers.ToList();
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
            var eventIds = _resolvers.Select(r => r.ConvertToEventId(messageId))
                                        .Where(id => id.HasValue)
                                        .Select(id => id!.Value).ToArray();

            var automats = _automatRepository.TriggeredByEvent(eventIds);

            foreach (var automat in automats)
            {
                await _automationService.ExecuteAutomatAsync(automat);
                automat.LastRun = DateTimeOffset.UtcNow;
                await _automatRepository.Update(automat);
            }
        }
    }
}
