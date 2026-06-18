namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class CategoriesService(BalkanAirDbContext db) : CrudService<Category>(db), ICategoriesService
{
    protected override DbSet<Category> Set => Db.Categories;
    protected override void MarkDeleted(Category e) => e.IsDeleted = true;
}
