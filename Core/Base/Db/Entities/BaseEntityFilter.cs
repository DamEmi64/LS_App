using Base.Helpers;
using System.ComponentModel;

namespace Base
{
    public abstract class BaseEntityFilter<TEntity> : IEntityFilter<TEntity> where TEntity : Entity
    {
        public int PageSize { get; set; } = 10;

        public int Page { get; set; }

        public abstract IEnumerable<TEntity> Filter(IEnumerable<TEntity> data, out int? count);

        protected IEnumerable<T> Sort<T>(IEnumerable<T> objects, string column, string order) where T : Entity
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
