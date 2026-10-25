using Base;
using Base.Helpers;
using System.ComponentModel;

namespace Files.Application.Filters
{
    public class FileFilter : BaseEntityFilter<Domain.Entities.File>
    {
        public required string Order { get; set; }
        public string? OrderBy { get; set; }
        public string? Title { get; set; }
        public string? Locaction { get; set; }
        public int? FileType { get; set; }
        public string? Subject { get; set; }
        public int? Year { get; set; }
        public int? Semester { get; set; }
        public bool IncludeImages { get; set; } = false;

        public override IEnumerable<Domain.Entities.File> Filter(IEnumerable<Domain.Entities.File> data, out int? count)
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

            count = data.Count();

            return data.Skip(Page * PageSize).Take(PageSize);
        }
      
    }
}