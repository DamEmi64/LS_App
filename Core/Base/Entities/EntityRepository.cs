using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Base
{
    /// <summary>
    ///     Default entity repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityRepository<T> where T : Entity
    {
        /// <summary>
        ///     Add Entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Add(T entity);

        /// <summary>
        ///     Get all entities (without includes)
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetAll();

        /// <summary>
        ///     Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T?> Get(Guid id);

        /// <summary>
        ///     Remove entity
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Remove(Guid id);

        /// <summary>
        ///     Update entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(T entity);
    }

    /// <summary>
    ///     Default entity repository instance
    /// </summary>
    /// <typeparam name="T1">Entity</typeparam>
    /// <typeparam name="T2">Database context with <see cref="T1"/></typeparam>
    public abstract class EntityRepository<T1, T2> : IEntityRepository<T2> where T2 : Entity where T1 : DbContextBase<T1>
    {
        public T1 DbContext { get; }

        protected EntityRepository(T1 dbContext)
        {
            DbContext = dbContext;
        }

        public virtual IEnumerable<T2> GetAll()
        {
            return DbContext.Set<T2>();
        }

        public virtual async Task<T2?> Get(Guid id)
        {
            return await DbContext.Set<T2>().FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task Update(T2 entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Add(T2 entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Remove(Guid id)
        {
            var entity = await Get(id);

            if (entity is not null)
            {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
            }
        }
    }

    /// <summary>
    ///     Default entity repository instance with caching
    /// </summary>
    /// <typeparam name="T1">Entity</typeparam>
    /// <typeparam name="T2">Database context with <see cref="T1"/></typeparam>
    public abstract class EntityCachedRepository<T1, T2> : EntityRepository<T1,T2> where T2 : Entity where T1 : DbContextBase<T1>
    {
        public IMemoryCache Cache { get; }

        protected EntityCachedRepository(T1 dbContext, IMemoryCache memoryCache)
            :base (dbContext)
        {
            Cache = memoryCache;
        }

        public override IEnumerable<T2> GetAll() 
            => Cache.GetOrCreate($"{typeof(T2).Name}_ALL", entry => base.GetAll()) ?? base.GetAll();

        public override async Task<T2?> Get(Guid id)
            => await Cache.GetOrCreateAsync($"{typeof(T2).Name}_{id}", entry => base.Get(id)) ?? await base.Get(id);

        public override async Task Update(T2 entity)
        {
            DbContext.Update(entity);
            await DbContext.SaveChangesAsync();
            var entry = Cache.CreateEntry($"{typeof(T2).Name}_{entity.Id}");
            entry.SetValue(entry);
        }

        public override async Task Add(T2 entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
            var entry = Cache.CreateEntry($"{typeof(T2).Name}_{entity.Id}");
            entry.SetValue(entry);
        }

        public override async Task Remove(Guid id)
        {
            var entity = await Get(id);

            if (entity is not null)
            {
                DbContext.Remove(entity);
                await DbContext.SaveChangesAsync();
                Cache.Remove($"{typeof(T2).Name}_{id}");
            }
        }
    }
}