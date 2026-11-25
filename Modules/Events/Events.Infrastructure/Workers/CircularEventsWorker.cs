using Base;
using Events.Domain.Dictionaries;
using Events.Domain.Entities;
using Events.Domain.Enums;
using Events.Domain.Repositories;
using SharedEvents;

namespace Events.Infrastructure.Workers
{
    public class CircularEventsWorker : IWorker
    {
        private readonly IEventRepository _eventRepository;
        private readonly IConnect _connect;

        public CircularEventsWorker(IEventRepository eventRepository, IConnect connect)
        {
            _eventRepository = eventRepository;
            _connect = connect;
        }

        public string Cron => "0 0 * * *";

        public void Execute()
        {
            var today = DateTime.Today;
            var tasks = _eventRepository.GetCircuralEvents()
                .Select(ev => ProcessEvent(ev, today));

            Task.WhenAll(tasks).GetAwaiter().GetResult();
        }

        private async Task ProcessEvent(Event ev, DateTime today)
        {
            if (ev.EventDate is not DateTime eventDate)
                return;

            var anchor = ev.CircularStartDate ?? eventDate;
            var occurrence = GetLatestOccurrence(anchor, today, ev.CircularPeriod);
            if (occurrence is null || occurrence.Value.Date <= eventDate.Date)
                return;

            _eventRepository.UpdateEventDate(ev.Id, occurrence.Value);
            await _connect.SendEventLater(
                new LogEvent(EventLogs.CircularEventStarted, LogLevel.Info),
                occurrence.Value);
        }

        private static DateTime? GetLatestOccurrence(DateTime anchor, DateTime today, CircularPeriodEnum period)
        {
            if (anchor.Date > today.Date)
                return null;

            var occurrences = period switch
            {
                CircularPeriodEnum.Daily => (today.Date - anchor.Date).Days,
                CircularPeriodEnum.Weekly => (today.Date - anchor.Date).Days / 7,
                CircularPeriodEnum.Monthly => MonthsBetween(anchor.Date, today.Date),
                CircularPeriodEnum.Yearly => today.Year - anchor.Year,
                _ => 0
            };

            var candidate = period switch
            {
                CircularPeriodEnum.Daily => anchor.AddDays(occurrences),
                CircularPeriodEnum.Weekly => anchor.AddDays(occurrences * 7),
                CircularPeriodEnum.Monthly => anchor.AddMonths(occurrences),
                CircularPeriodEnum.Yearly => anchor.AddYears(occurrences),
                _ => anchor
            };

            while (candidate.Date > today.Date)
            {
                occurrences--;
                candidate = period switch
                {
                    CircularPeriodEnum.Daily => anchor.AddDays(occurrences),
                    CircularPeriodEnum.Weekly => anchor.AddDays(occurrences * 7),
                    CircularPeriodEnum.Monthly => anchor.AddMonths(occurrences),
                    CircularPeriodEnum.Yearly => anchor.AddYears(occurrences),
                    _ => anchor
                };
            }

            return candidate;
        }

        private static int MonthsBetween(DateTime start, DateTime end)
        {
            var months = (end.Year - start.Year) * 12 + end.Month - start.Month;
            return start.AddMonths(months).Date > end.Date ? months - 1 : months;
        }
    }
}