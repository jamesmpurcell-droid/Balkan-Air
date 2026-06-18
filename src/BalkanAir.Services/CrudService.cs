namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public abstract class CrudService<TEntity>(BalkanAirDbContext db)
    : ICrudService<TEntity, int>
    where TEntity : class
{
    protected BalkanAirDbContext Db { get; } = db;

    protected abstract DbSet<TEntity> Set { get; }
    protected abstract void MarkDeleted(TEntity entity);

    public async Task<int> AddAsync(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        Set.Add(entity);
        await Db.SaveChangesAsync();

        var idProp = Db.Entry(entity).Property("Id");
        return (int)idProp.CurrentValue!;
    }

    public async Task<TEntity?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return await Set.FindAsync(id);
    }

    public IQueryable<TEntity> GetAll() => Set.AsQueryable();

    public async Task<TEntity?> UpdateAsync(int id, Action<TEntity> applyUpdates)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentNullException.ThrowIfNull(applyUpdates);

        var entity = await Set.FindAsync(id);
        if (entity is null) return null;

        applyUpdates(entity);
        await Db.SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity?> SoftDeleteAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        var entity = await Set.FindAsync(id);
        if (entity is null) return null;

        MarkDeleted(entity);
        await Db.SaveChangesAsync();
        return entity;
    }
}
