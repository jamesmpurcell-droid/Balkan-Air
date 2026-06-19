namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class RoutesService(BalkanAirDbContext db) : CrudService<Route>(db), IRoutesService
{
    protected override DbSet<Route> Set => Db.Routes;
    protected override void MarkDeleted(Route e) => e.IsDeleted = true;
}
