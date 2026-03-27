using Base;
using Base.Helpers;
using System.ComponentModel;
using System.Domain.Entities;

namespace System.Application.Filters
{
    public class ProcessFilter : IEntityFilter<Process>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public ProgressStatus? Status { get; set; }
        public DateTimeOffset? From { get; set; }
        public DateTimeOffset? To { get; set; }

        public IEnumerable<Process> Filter(IEnumerable<Process> data)
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

            return data.Skip(Page * PageSize).Take(PageSize);
        }

        private IEnumerable<T> Sort<T>(IEnumerable<T> objects, string column, string order) where T : Entity
        {
            var prop = TypeDescriptor.GetProperties(typeof(T)).Find(column, true);

            if (prop is null)
            {
                return objects;
            }

            if (order == "asc")
            {
                objects = objects.OrderBy(prop.GetValue);
            }
            else
            {
                objects = objects.OrderByDescending(prop.GetValue);
            }

            return objects;
        }
    }
}