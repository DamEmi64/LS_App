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
        public DateRange? Start { get; set; }
        public DateRange? End { get; set; }

        public IEnumerable<Story> Filter(IEnumerable<Story> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title, StringComparison.CurrentCultureIgnoreCase));
            }

            if (Start is not null)
            {
                if (Start.From.HasValue)
                {
                    var from = Start.From.Value;
                    data = data.Where(x => x.EndDate >= from);
                }

                if (Start.To.HasValue)
                {
                    var to = Start.To.Value;
                    data = data.Where(x => x.StartDate <= to);
                }
            }
            if (End is not null)
            {
                if (End.From.HasValue)
                {
                    var from = End.From.Value;
                    data = data.Where(x => x.EndDate >= from);
                }

                if (End.To.HasValue)
                {
                    var to = End.To.Value;
                    data = data.Where(x => x.StartDate <= to);
                }
            }

            if (!string.IsNullOrEmpty(OrderBy) && !string.IsNullOrEmpty(Order))
            {
                data = Sort(data, OrderBy, Order);
            }

            count = data.Count();

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