namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class FaresService(BalkanAirDbContext db) : CrudService<Fare>(db), IFaresService
{
    protected override DbSet<Fare> Set => Db.Fares;
    protected override void MarkDeleted(Fare e) => e.IsDeleted = true;
}
