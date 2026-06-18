namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class FlightsService(BalkanAirDbContext db) : CrudService<Flight>(db), IFlightsService
{
    protected override DbSet<Flight> Set => Db.Flights;
    protected override void MarkDeleted(Flight e) => e.IsDeleted = true;
}
