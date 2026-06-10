using Base;
using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class HistoryFilter : IEntityFilter<CommunicationHistory>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string? Subject { get; set; }
        public string? Receiver { get; set; }
        public DateRange? SentDate { get; set; }

        public IEnumerable<CommunicationHistory> Filter(IEnumerable<CommunicationHistory> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Subject))
            {
                data = data.Where(x => x.Subject.Contains(Subject));
            }

            if (!string.IsNullOrEmpty(Receiver))
            {
                data = data.Where(x => x.Recipient.Contains(Receiver));
            }

            if (SentDate is not null)
            {
                if (SentDate.From is not null)
                {
                    data = data.Where(x => x.Date >= SentDate.From);
                }
                if (SentDate.To is not null)
                {
                    data = data.Where(x => x.Date <= SentDate.To);
                }
            }

            count = data.Count();

            data = data.Skip(PageSize * (Page - 1)).Take(PageSize);

            return data;
        }
    }
}