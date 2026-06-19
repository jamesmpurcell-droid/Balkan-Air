namespace BalkanAir.Services.Tests;

using BalkanAir.Domain.Entities;
using BalkanAir.Domain.Enums;

public class BookingsServiceTests
{
    private static Booking MakeBooking(string code = "ABC123") => new()
    {
        ConfirmationCode = code,
        DateOfBooking = DateTime.UtcNow,
        Row = 1,
        SeatNumber = "A",
        TotalPrice = 150m,
        TravelClassId = 1,
        Status = BookingStatus.Confirmed,
        UserId = "user-1",
        LegInstanceId = 1
    };

    [Fact]
    public async Task AddAsync_ShouldReturnBookingId()
    {
        using var db = TestDbContextFactory.Create();
        var svc = new BookingsService(db);

        var id = await svc.AddAsync(MakeBooking());

        Assert.Equal(1, id);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldMarkAsDeleted()
    {
        using var db = TestDbContextFactory.Create();
        db.Bookings.Add(MakeBooking());
        await db.SaveChangesAsync();

        var svc = new BookingsService(db);
        var deleted = await svc.SoftDeleteAsync(1);

        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
    }

    [Fact]
    public async Task SoftDeleteAsync_NotFound_ShouldReturnNull()
    {
        using var db = TestDbContextFactory.Create();
        var svc = new BookingsService(db);

        var result = await svc.SoftDeleteAsync(999);

        Assert.Null(result);
    }
}
