namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class BaggageService(BalkanAirDbContext db) : CrudService<Baggage>(db), IBaggageService
{
    protected override DbSet<Baggage> Set => Db.Baggages;
    protected override void MarkDeleted(Baggage e) => e.IsDeleted = true;
}
