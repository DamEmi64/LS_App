using Automation.Domain.Repositories;
using Automation.Infrastructure.Services.NotifyListener.Command;
using Base;
using Base.Automation;
using MediatR;

namespace Automation.Infrastructure.Services.NotifyListener
{
    public class NotifyListener : INotifyListener
    {
        private readonly IAutomatRepository _automatRepository;
        private readonly List<IAutomationResolver> _resolvers;
        private readonly IMediator _mediator;

        public NotifyListener(IAutomatRepository automatRepository, IEnumerable<IAutomationResolver> resolvers, IMediator mediator)
        {
            _automatRepository = automatRepository;
            _resolvers = resolvers.ToList();
            _mediator = mediator;
        }

        public Task Notify<T>(T @event) where T : NotifyEvent => CheckAutomats(@event);

        private async Task CheckAutomats<T>(T @event) where T : NotifyEvent
        {
            var eventIds = _resolvers.Select(r => r.ConvertToEventId(@event))
                                        .Where(id => id.HasValue)
                                        .Select(id => id!.Value).ToArray();

            var automats = _automatRepository.TriggeredByEvent(eventIds);

            foreach (var automat in automats)
            {
                await _mediator.Send(new AutomationExecuter { Automat = automat });
                automat.LastRun = DateTimeOffset.UtcNow;
                await _automatRepository.Update(automat);
            }
        }
    }
}
