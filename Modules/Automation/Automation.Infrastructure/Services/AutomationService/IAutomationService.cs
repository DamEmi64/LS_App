using Automation.Domain.Entities;

namespace Automation.Infrastructure.Services.AutomationService
{
    public interface IAutomationService
    {
        System.Threading.Tasks.Task AddOrUpdateAutomat(Automat automat);
        void ExecuteAutomat(Guid id, string title, Automat automat);
        System.Threading.Tasks.Task ExecuteAutomatAsync(Automat automat);
        System.Threading.Tasks.Task RemoveAutomat(Automat automat);
        System.Threading.Tasks.Task TurnOffAutomat(Automat automat);
        System.Threading.Tasks.Task TurnOnAutomat(Automat automat);
    }
}