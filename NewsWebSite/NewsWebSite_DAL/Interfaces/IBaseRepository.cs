using NewsWebSite_DAL.Models;

namespace NewsWebSite_DAL.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        IQueryable<TEntity> GetAllAsync();

        Task<TEntity> UpdateEntityAsync(TEntity entity);

        Task<TEntity> AddEntityAsync(TEntity entity);

        Task<TEntity> GetByIdAsync(Guid id);

        Task DeleteEntityAsync(Guid id);
    }
}
