using Base;
using Base.Helpers;
using RPG.Domain.Entities;
using System.ComponentModel;

namespace RPG.Application.Filters
{
    public class ChapterFilter : IEntityFilter<Chapter>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public string? Title { get; set; }
        public DateTimeOffset? StartFrom { get; set; }
        public DateTimeOffset? StartTo { get; set; }
        public DateTimeOffset? EndFrom { get; set; }
        public DateTimeOffset? EndTo { get; set; }

        public IEnumerable<Chapter> Filter(IEnumerable<Chapter> data)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }
            if (StartFrom is not null)
            {
                data = data.Where(x => x.Sessions.FirstOrDefault()?.Start >= StartFrom);
            }
            if (StartTo is not null)
            {
                data = data.Where(x => x.Sessions.FirstOrDefault()?.Start <= StartTo);
            }
            if (EndFrom is not null)
            {
                data = data.Where(x => x.Sessions.LastOrDefault()?.End >= EndFrom);
            }
            if (EndTo is not null)
            {
                data = data.Where(x => x.Sessions.LastOrDefault()?.End <= EndTo);
            }

            data = data.OrderBy(x => x.Order);

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