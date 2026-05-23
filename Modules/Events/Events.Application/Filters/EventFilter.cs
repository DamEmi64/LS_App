using Base;
using Base.Helpers;
using Events.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Events.Application.Filters
{
    public class EventFilter : IEntityFilter<Event>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public string? Title { get; set; }
        public DateRange? Date { get; set; }
        public int Category { get; set; }

        public IEnumerable<Event> Filter(IEnumerable<Event> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }

            if (Category != 0)
            {
                data = data.Where(x=>x.CategoryId == Category);
            }

            if (Date is not null)
            {
                if (Date.From.HasValue)
                {
                    var from = Date.From.Value;
                    data = data.Where(x => x.EventDate >= from);
                }

                if (Date.To.HasValue)
                {
                    var to = Date.To.Value;
                    data = data.Where(x => x.EventDate <= to);
                }
            }

            count = data.Count();

            return data.Skip(Page * PageSize).Take(PageSize);
        }
    }
}
