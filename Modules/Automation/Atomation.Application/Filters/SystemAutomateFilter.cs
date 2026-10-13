using Automation.Domain.Entities;
using Base;
using Base.Helpers;
using System.ComponentModel;

namespace Automation.Application.Filters
{
    public class SystemAutomateFilter : IEntityFilter<SystemAutomat>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public required string Order { get; set; }
        public string? OrderBy { get; set; }

        public IEnumerable<SystemAutomat> Filter(IEnumerable<SystemAutomat> data, out int? count)
        {
            if (!string.IsNullOrEmpty(OrderBy) && !string.IsNullOrEmpty(Order))
            {
                data = Sort(data, OrderBy, Order);
            }

            count = data.Count();

            return data.Skip(Page * PageSize).Take(PageSize);
        }

        private IEnumerable<T> Sort<T>(IEnumerable<T> objects, string column, string order) where T : class
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
