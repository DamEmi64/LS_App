using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class TemplateFilter : IEntityFilter<Template>
    {
        public string? Title { get; set; }

        public int PageSize { get; set; }

        public int Page { get; set; }

        public IEnumerable<Template> Filter(IEnumerable<Template> data, out int? count)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Subject.Contains(Title));
            }

            count = data.Count();

            return data;
        }
    }
}