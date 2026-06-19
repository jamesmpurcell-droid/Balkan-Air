namespace BalkanAir.Services.Contracts;

public interface ICrudService<TEntity, TKey> where TEntity : class
{
    Task<TKey> AddAsync(TEntity entity);
    Task<TEntity?> GetByIdAsync(TKey id);
    IQueryable<TEntity> GetAll();
    Task<TEntity?> UpdateAsync(TKey id, Action<TEntity> applyUpdates);
    Task<TEntity?> SoftDeleteAsync(TKey id);
}
