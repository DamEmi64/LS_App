using Base;
using RPG.Domain.Entities;

namespace RPG.Application.Filters
{
    public class HeroFilter : BaseEntityFilter<Hero>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public override IEnumerable<Hero> Filter(IEnumerable<Hero> data, out int? count)
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
    }
}