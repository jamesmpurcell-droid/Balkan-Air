namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class AircraftManufacturersService(BalkanAirDbContext db) : CrudService<AircraftManufacturer>(db), IAircraftManufacturersService
{
    protected override DbSet<AircraftManufacturer> Set => Db.AircraftManufacturers;
    protected override void MarkDeleted(AircraftManufacturer e) => e.IsDeleted = true;
}
