namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class FlightStatusesService(BalkanAirDbContext db) : CrudService<FlightStatus>(db), IFlightStatusesService
{
    protected override DbSet<FlightStatus> Set => Db.FlightStatuses;
    protected override void MarkDeleted(FlightStatus e) => e.IsDeleted = true;
}
