namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class FlightLegsService(BalkanAirDbContext db) : CrudService<FlightLeg>(db), IFlightLegsService
{
    protected override DbSet<FlightLeg> Set => Db.FlightLegs;
    protected override void MarkDeleted(FlightLeg e) => e.IsDeleted = true;
}
