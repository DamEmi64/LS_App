using Automation.Domain.Entities;
using Automation.Infrastructure.Services.AutomationService;
using MediatR;

namespace Automation.Infrastructure.Services.NotifyListener.Command
{
    public class AutomationExecuter : IRequest
    {
        public required Automat Automat { get; set; }

        public class AutomationExecuterHandler : IRequestHandler<AutomationExecuter>
        {
            private readonly IAutomationService _automationService;
            public AutomationExecuterHandler(IAutomationService automationService)
            {
                _automationService = automationService;
            }

            public async System.Threading.Tasks.Task Handle(AutomationExecuter request, CancellationToken cancellationToken)
            {
                await _automationService.ExecuteAutomatAsync(request.Automat);
            }
        }
    }
}
