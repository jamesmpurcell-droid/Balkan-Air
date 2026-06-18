namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class AirportsService(BalkanAirDbContext db) : CrudService<Airport>(db), IAirportsService
{
    protected override DbSet<Airport> Set => Db.Airports;
    protected override void MarkDeleted(Airport e) => e.IsDeleted = true;
}
