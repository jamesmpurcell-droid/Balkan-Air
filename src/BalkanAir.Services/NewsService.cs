namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class NewsService(BalkanAirDbContext db) : CrudService<News>(db), INewsService
{
    protected override DbSet<News> Set => Db.News;
    protected override void MarkDeleted(News e) => e.IsDeleted = true;
}
