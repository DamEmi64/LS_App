using Automation.Domain.Repositories;
using AutomationBase;
using Base;
using Microsoft.Extensions.DependencyInjection;

namespace Automation.Infrastructure.Services.NotifyListener
{
    public class SystemAutomateListener : INotifyListener
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISystemAutomateRepository _systemAutomateRepository;

        public SystemAutomateListener(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _systemAutomateRepository = serviceProvider.GetRequiredService<ISystemAutomateRepository>();
        }

        public async Task Notify<T>(T @event) where T : NotifyEvent
        {
            var systemAutomates = _serviceProvider.GetServices<ISystemAutomate>();
            var activeAutomates = _systemAutomateRepository.GetAll().Where(x => x.Active).ToList();

            foreach (var automate in activeAutomates)
            {
                var systemAutomate = systemAutomates.FirstOrDefault(x => x.Key == automate.Key);
                if (systemAutomate != null && systemAutomate.CanExecute(@event))
                {
                    await systemAutomate.Execute(@event, automate.Configuration ?? "{}");
                }
            }
        }
    }
}
