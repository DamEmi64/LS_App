using Automation.Domain.Entities;
using Base;

namespace Automation.Application.Filters
{
    public class AutomatonFilter : BaseEntityFilter<Automat>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }

        public override IEnumerable<Automat> Filter(IEnumerable<Automat> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title, StringComparison.CurrentCultureIgnoreCase));
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
