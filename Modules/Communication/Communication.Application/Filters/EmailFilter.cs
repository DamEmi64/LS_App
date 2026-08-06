using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class EmailFilter : IEntityFilter<Email>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string? Subject { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public int Status { get; set; }

        public IEnumerable<Email> Filter(IEnumerable<Email> data, out int? count)
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

            if (Status > 0)
            {
                data = data.Where(x => x.Status == Status);
            }

            count = data.Count();

            data = data.Skip(PageSize * (Page - 1)).Take(PageSize);

            return data;
        }
    }
}