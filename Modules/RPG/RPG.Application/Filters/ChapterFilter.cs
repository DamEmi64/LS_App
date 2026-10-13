using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class ChapterFilter : BaseEntityFilter<Chapter>
    {
        public string? Title { get; set; }
        public DateRange? Start { get; set; }
        public DateRange? End { get; set; }

        public override IEnumerable<Chapter> Filter(IEnumerable<Chapter> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }

            if (Start is not null)
            {
                if (Start.From.HasValue)
                {
                    var from = Start.From.Value;
                    data = data.Where(x => x.Sessions.Any(y => y.Start >= from));
                }

                if (Start.To.HasValue)
                {
                    var to = Start.To.Value;
                    data = data.Where(x => x.Sessions.Any(y => y.Start <= to));
                }
            }
            if (End is not null)
            {
                if (End.From.HasValue)
                {
                    var from = End.From.Value;
                    data = data.Where(x => x.Sessions.Any(y => y.End >= from));
                }

                if (End.To.HasValue)
                {
                    var to = End.To.Value;
                    data = data.Where(x => x.Sessions.Any(y => y.End <= to));
                }
            }

            if (Start is not null)
            {
                var from = Start.From ?? DateTime.MinValue;
                var to = Start.To ?? DateTime.MaxValue;

                data = data.Where(x => x.Sessions.Any(y => y.Start >= from && y.Start <= to));
            }
            if (End is not null)
            {
                var from = End.From ?? DateTime.MinValue;
                var to = End.To ?? DateTime.MaxValue;
                data = data.Where(x => x.Sessions.Any(y => y.End >= from && y.End <= to));
            }

            data = data.OrderBy(x => x.Order);

            count = data.Count();

            return data.Skip(Page * PageSize).Take(PageSize);
        }
    }
}