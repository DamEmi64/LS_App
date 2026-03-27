using Base.Helpers;
using Communication.Domain.Entities;

namespace Communication.Application.Filters
{
    public class TemplateFilter : IEntityFilter<Template>
    {
        public string? Title { get; set; }

        public IEnumerable<Template> Filter(IEnumerable<Template> data)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Subject.Contains(Title));
            }

            return data;
        }
    }
}