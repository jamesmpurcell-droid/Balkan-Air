namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class TravelClassesService(BalkanAirDbContext db) : CrudService<TravelClass>(db), ITravelClassesService
{
    protected override DbSet<TravelClass> Set => Db.TravelClasses;
    protected override void MarkDeleted(TravelClass e) => e.IsDeleted = true;
}
