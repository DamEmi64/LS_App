using Base;
using Base.Automation;
using Events.Domain.Dictionaries;
using Events.Domain.Enums;
using Events.Domain.Repositories;
using Events.Infrastructure.Jobs;
using Newtonsoft.Json;

namespace Events.Infrastructure.Services.AutomationResolver
{
    public class EventAutomationResolver : IAutomationResolver
    {
        private readonly IControllerService _controllerService;
        private readonly IEventRepository _eventRepository;

        public EventAutomationResolver(IControllerService controllerService, IEventRepository eventRepository)
        {
            _controllerService = controllerService;
            _eventRepository = eventRepository;
        }

        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks)
        {
            foreach (var task in tasks)
            {
                if (task.Operation == Operations.SendReminder)
                {
                    var autoReminderSettings = JsonConvert.DeserializeObject<AutoReminderEnum>(task.JsonData ?? string.Empty);
                    var lastAddedEvent = _eventRepository.GetLastAdded();
                    if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                        continue;

                    foreach (var participate in lastAddedEvent.Participates)
                    {
                        var job = new SendReminderJob
                        {
                            Event = lastAddedEvent,
                            Receiver = new UserData
                            {
                                UserId = participate.UserId,
                                Email = participate.Email,
                                Login = participate.Login
                            },
                            RequestDate = CalcDate(lastAddedEvent.EventDate ?? DateTime.Now, autoReminderSettings)
                        };
                        schema.AddJob(job);
                    }

                    task.Handled = true;
                }
                else if (task.Operation == Operations.SendInvitation)
                {
                    var lastAddedEvent = _eventRepository.GetLastAdded();
                    if (lastAddedEvent is null || lastAddedEvent.EventDate is null)
                        continue;

                    foreach (var user in _controllerService.Users)
                    {
                        var job = new SendInvitationJob
                        {
                            Event = lastAddedEvent,
                            Receiver = new UserData
                            {
                                UserId = user.UserId,
                                Email = user.Email,
                                Login = user.Login
                            },
                            RequestDate = DateTime.Now
                        };
                        schema.AddJob(job);
                    }

                    task.Handled = true;
                }
            }
        }

        private DateTime CalcDate(DateTime eventDate, AutoReminderEnum autoReminder)
        => autoReminder switch
        {
            AutoReminderEnum.Min15 => eventDate.AddMinutes(-15),
            AutoReminderEnum.Min30 => eventDate.AddMinutes(-30),
            AutoReminderEnum.Day1 => eventDate.AddDays(-1),
            AutoReminderEnum.Week1 => eventDate.AddDays(-7),
            AutoReminderEnum.Month1 => eventDate.AddMonths(-1),
            _ => throw new ArgumentOutOfRangeException(nameof(autoReminder), autoReminder, null)
        };
    }
}
