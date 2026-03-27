using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class StoryFilter : IEntityFilter<Story>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public DateTimeOffset? StartFrom { get; set; }
        public DateTimeOffset? StartTo { get; set; }
        public DateTimeOffset? EndFrom { get; set; }
        public DateTimeOffset? EndTo { get; set; }

        public IEnumerable<Story> Filter(IEnumerable<Story> data)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title, StringComparison.CurrentCultureIgnoreCase));
            }
            if (StartFrom is not null)
            {
                data = data.Where(x => x.StartDate >= StartFrom);
            }
            if (StartTo is not null)
            {
                data = data.Where(x => x.StartDate <= StartTo);
            }
            if (EndFrom is not null)
            {
                data = data.Where(x => x.EndDate >= EndFrom);
            }
            if (EndTo is not null)
            {
                data = data.Where(x => x.EndDate <= EndTo);
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