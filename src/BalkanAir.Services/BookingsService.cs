namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class BookingsService(BalkanAirDbContext db) : CrudService<Booking>(db), IBookingsService
{
    protected override DbSet<Booking> Set => Db.Bookings;
    protected override void MarkDeleted(Booking e) => e.IsDeleted = true;
}
