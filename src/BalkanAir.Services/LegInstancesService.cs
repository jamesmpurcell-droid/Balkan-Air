namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class LegInstancesService(BalkanAirDbContext db) : CrudService<LegInstance>(db), ILegInstancesService
{
    protected override DbSet<LegInstance> Set => Db.LegInstances;
    protected override void MarkDeleted(LegInstance e) => e.IsDeleted = true;
}
