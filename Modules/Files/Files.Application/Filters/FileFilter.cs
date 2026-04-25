using Base;
using Base.Helpers;
using System.ComponentModel;

namespace Files.Application.Filters
{
    public class FileFilter : IEntityFilter<Domain.Entities.File>
    {
        public int Page { get; set; }
        public int PageSize { get; set; } = 10;
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public string? Locaction { get; set; }
        public int? FileType { get; set; }
        public string? Subject { get; set; }
        public int? Year { get; set; }
        public int? Semester { get; set; }
        public bool IncludeImages { get; set; } = false;

        public IEnumerable<Domain.Entities.File> Filter(IEnumerable<Domain.Entities.File> data)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                data = data.Where(x => x.Title.Contains(Title));
            }
            if (!string.IsNullOrEmpty(Locaction))
            {
                data = data.Where(x => x.Locaction?.Contains(Locaction) ?? false);
            }
            if (!string.IsNullOrEmpty(Subject))
            {
                data = data.Where(x => x.AdditionalData?.Subject?.Contains(Subject) ?? false);
            }
            if (FileType is not null)
            {
                data = data.Where(x => x.FileType == FileType);
            }
            if (Year is not null)
            {
                data = data.Where(x => x.AdditionalData?.Year == Year);
            }
            if (Semester is not null)
            {
                data = data.Where(x => x.AdditionalData?.Semester == Semester);
            }

            if (!string.IsNullOrEmpty(Order) && !string.IsNullOrEmpty(OrderBy))
            {
                data = Sort(data, OrderBy, Order);
            }

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