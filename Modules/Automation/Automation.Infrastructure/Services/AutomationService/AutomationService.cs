using Automation.Domain.Entities;
using Automation.Domain.Repositories;
using AutomationBase.Dictionaries;
using Base;
using Base.Automation;
using Hangfire;
using System.ComponentModel.DataAnnotations;

namespace Automation.Infrastructure.Services.AutomationService
{
    public class AutomationService : IAutomationService
    {
        private readonly IAutomatRepository _automatRepository;
        private readonly IJobEngine _jobEngine;
        private readonly IEnumerable<IAutomationResolver> _resolvers;
        private readonly RecurringJobManager _recurringJobManager;

        public AutomationService(IJobEngine jobEngine, IEnumerable<IAutomationResolver> resolvers, IAutomatRepository automatRepository)
        {
            _jobEngine = jobEngine;
            _resolvers = resolvers;
            _automatRepository = automatRepository;
            _recurringJobManager = new RecurringJobManager();
        }

        public async System.Threading.Tasks.Task AddOrUpdateAutomat(Automat automat)
        {
            foreach (var trigger in automat.Triggers)
            {
                if (trigger.EventId == AutomationEvents.Cron)
                {
                    _recurringJobManager.AddOrUpdate(automat.Id.ToString(), () => ExecuteAutomat(automat.Id, automat.Title, automat), trigger.Cron ?? Cron.Hourly());
                }
            }

            var dbAutomat = await _automatRepository.Get(automat.Id);

            if (dbAutomat is null)
            {
                await _automatRepository.Add(automat);
            }
            else
            {

                await _automatRepository.Update(automat);
            }
        }

        public async System.Threading.Tasks.Task TurnOffAutomat(Automat automat)
        {
            foreach (var trigger in automat.Triggers)
            {
                if (trigger.EventId == AutomationEvents.Cron)
                {
                    _recurringJobManager.RemoveIfExists(automat.Id.ToString());
                }
            }

            automat.Active = false;
            await _automatRepository.Update(automat);
        }

        public async System.Threading.Tasks.Task TurnOnAutomat(Automat automat)
        {
            foreach (var trigger in automat.Triggers)
            {
                if (trigger.EventId == AutomationEvents.Cron)
                {
                    _recurringJobManager.AddOrUpdate(automat.Id.ToString(), () => ExecuteAutomat(automat.Id, automat.Title, automat), trigger.Cron ?? Cron.Hourly());
                }
            }
            automat.Active = true;
            await _automatRepository.Update(automat);
        }

        public async System.Threading.Tasks.Task RemoveAutomat(Automat automat)
        {
            foreach (var trigger in automat.Triggers)
            {
                if (trigger.EventId == AutomationEvents.Cron)
                {
                    _recurringJobManager.RemoveIfExists(automat.Id.ToString());
                }
            }

            await _automatRepository.Remove(automat.Id);
        }

        [Display(Name = "[AUTOMAT] (@0) @1")]
        public void ExecuteAutomat(Guid id, string title, Automat automat)
        {
            ExecuteAutomatAsync(automat).Wait();
        }


        public async System.Threading.Tasks.Task ExecuteAutomatAsync(Automat automat)
        {
            var schema = CreateSchema(automat);

            await _jobEngine.Execute(schema, new UserData { Email = "automat", Id = 0, UserId = Guid.Empty.ToString(), Login = "automat", Role = "admin" });
            automat.LastRun = DateTimeOffset.UtcNow;
            await _automatRepository.Update(automat);
        }

        private IProcessSchema CreateSchema(Automat automat)
        {
            var schema = _jobEngine.Create($"[AUTOMAT] {automat.Title} - {DateTime.Now.ToString()}");

            var automationTasks = automat.Tasks.Select(x => new AutomationTask
            {
                Operation = x.OperationId,
                Order = x.Order,
                JsonData = x.Data
            })
            .OrderBy(x => x.Order);

            foreach (var resolver in _resolvers)
            {
                resolver.Resolve(schema, automationTasks.Where(x => !x.Handled));
            }

            return schema;
        }
    }
}
