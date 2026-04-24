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
}