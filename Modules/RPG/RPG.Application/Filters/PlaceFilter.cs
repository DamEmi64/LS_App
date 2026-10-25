using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class PlaceFilter : BaseEntityFilter<Place>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }

        public override IEnumerable<Place> Filter(IEnumerable<Place> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
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