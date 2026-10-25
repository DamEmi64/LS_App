using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class StoryFilter : BaseEntityFilter<Story>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public DateRange? Start { get; set; }
        public DateRange? End { get; set; }

        public  override IEnumerable<Story> Filter(IEnumerable<Story> data, out int? count)
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
    }
}