using Base;
using Base.Helpers;
using System.ComponentModel;
using System.Domain.Entities;

namespace System.Application.Filters
{
    public class ProcessFilter : BaseEntityFilter<ProcessRead>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public ProgressStatus? Status { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }

        public override IEnumerable<ProcessRead> Filter(IEnumerable<ProcessRead> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }
            if (Status is not null)
            {
                data = data.Where(x => x.Status == Status);
            }
            if (From is not null)
            {
                data = data.Where(x => x.StartDate > From);
            }
            if (To is not null)
            {
                data = data.Where(x => x.StartDate < To);
            }

            if (!string.IsNullOrEmpty(OrderBy) && !string.IsNullOrEmpty(Order))
            {
                data = Sort(data, OrderBy, Order);
            }

            count = data.Count();

            return data.Skip(Page * PageSize).Take(PageSize);
        }
    }
}