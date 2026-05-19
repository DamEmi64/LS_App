using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class HeroFilter : IEntityFilter<Hero>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public IEnumerable<Hero> Filter(IEnumerable<Hero> data, out int? count)
        {
            if (!string.IsNullOrEmpty(FirstName))
            {
                data = data.Where(x => x.FirstName.Contains(FirstName));
            }

            if (!string.IsNullOrEmpty(LastName))
            {
                data = data.Where(x => x.LastName.Contains(LastName));
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