namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class SeatsService(BalkanAirDbContext db) : CrudService<Seat>(db), ISeatsService
{
    protected override DbSet<Seat> Set => Db.Seats;
    protected override void MarkDeleted(Seat e) => e.IsDeleted = true;
}
