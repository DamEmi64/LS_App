using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class EmailFilter : IEntityFilter<Email>
    {
        public string? Subject { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public DateTimeOffset? SentDateFrom { get; set; }
        public DateTimeOffset? SentDateTo { get; set; }

        public IEnumerable<Email> Filter(IEnumerable<Email> data)
        {
            if (!string.IsNullOrEmpty(Subject))
            {
                data = data.Where(x => x.Subject.Contains(Subject));
            }
            if (!string.IsNullOrEmpty(Sender))
            {
                data = data.Where(x => x.Sender.Contains(Sender));
            }
            if (!string.IsNullOrEmpty(Receiver))
            {
                data = data.Where(x => x.Recipient.Contains(Receiver));
            }
            if (SentDateFrom is not null)
            {
                data = data.Where(x => x.SentDate > SentDateFrom);
            }
            if (SentDateTo is not null)
            {
                data = data.Where(x => x.SentDate < SentDateTo);
            }

            return data;
        }
    }
}