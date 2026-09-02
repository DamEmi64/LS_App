using Base;
using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class HistoryFilter : IEntityFilter<CommunicationRegistry>
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
        public string? Title { get; set; }
        public string? From { get; set; }
        public string? To { get; set; }
        public DateRange? SentDate { get; set; }

        public IEnumerable<CommunicationRegistry> Filter(IEnumerable<CommunicationRegistry> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }

            if (!string.IsNullOrEmpty(From))
            {
                data = data.Where(x => x.From.Contains(From));
            }

            if (!string.IsNullOrEmpty(To))
            {
                data = data.Where(x => x.To.Contains(To));
            }

            if (SentDate is not null)
            {
                if (SentDate.From is not null)
                {
                    data = data.Where(x => x.InsDate >= SentDate.From);
                }
                if (SentDate.To is not null)
                {
                    data = data.Where(x => x.InsDate <= SentDate.To);
                }
            }

            count = data.Count();

            data = data.Skip(PageSize * Page).Take(PageSize);

            return data;
        }
    }
}