using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Base
{
    /// <summary>
    ///     Default entity repository instance with caching
    /// </summary>
    /// <typeparam name="T1">Entity</typeparam>
    /// <typeparam name="T2">Database context with <see cref="T1"/></typeparam>
    public abstract class CachedEntityRepository<TEntity, TContext>
        : IEntityRepository<TEntity>
        where TEntity : Entity
        where TContext : DbContext
    {
        protected IMemoryCache Cache { get; }
        protected TContext DbContext { get; }

        protected virtual TimeSpan CacheDuration => TimeSpan.FromMinutes(5);
        public virtual long Size => 1;

        protected CachedEntityRepository(TContext dbContext, IMemoryCache cache)
        {
            DbContext = dbContext;
            Cache = cache;
        }

        protected virtual string GetAllKey() => $"{typeof(TEntity).Name}_ALL";
        protected virtual string GetKey(Guid id) => $"{typeof(TEntity).Name}_{id}";

        public IEnumerable<TEntity> GetAll()
        {
            return Cache.GetOrCreate(GetAllKey(), entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.SetSize(Size * 10);
                return GetAllInternal().ToList();
            })!;
        }

        public async Task<TEntity?> Get(Guid id)
        {
            return await Cache.GetOrCreateAsync(GetKey(id), async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;
                entry.SetSize(Size);
                return await GetInternal(id);
            });
        }

        public async Task Add(TEntity entity)
        {
            await AddInternal(entity);

            Cache.Set(GetKey(entity.Id), entity, CacheDuration);
            Cache.Remove(GetAllKey()); // invalidate list
        }

        public async Task Update(TEntity entity)
        {
            await UpdateInternal(entity);

            Cache.Set(GetKey(entity.Id), entity, CacheDuration);
            Cache.Remove(GetAllKey()); // invalidate list
        }

        public async Task Remove(Guid id)
        {
            await RemoveInternal(id);

            Cache.Remove(GetKey(id));
            Cache.Remove(GetAllKey()); // invalidate list
        }

        // Internal methods (override-friendly)

        protected virtual IQueryable<TEntity> GetAllInternal()
            => DbContext.Set<TEntity>();

        protected virtual Task<TEntity?> GetInternal(Guid id) => DbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        protected virtual async Task AddInternal(TEntity entity)
        {
            await DbContext.AddAsync(entity);
            await DbContext.SaveChangesAsync();
        }

        protected virtual async Task UpdateInternal(TEntity entity)
        {
            DbContext.ChangeTracker.Clear();

            // Attach the entity and mark it as modified
            DbContext.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
        }

        protected virtual async Task RemoveInternal(Guid id)
        {
            var entity = await GetInternal(id);
            if (entity is null) return;

            DbContext.Remove(entity);
            await DbContext.SaveChangesAsync();
        }
    }
}