namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class AircraftsService(BalkanAirDbContext db) : CrudService<Aircraft>(db), IAircraftsService
{
    protected override DbSet<Aircraft> Set => Db.Aircraft;
    protected override void MarkDeleted(Aircraft e) => e.IsDeleted = true;
}
