namespace BalkanAir.Services.Tests;

using BalkanAir.Domain.Entities;

public class FlightsServiceTests
{
    [Fact]
    public async Task AddAsync_ShouldPersistAndReturnId()
    {
        using var db = TestDbContextFactory.Create();
        var svc = new FlightsService(db);

        var id = await svc.AddAsync(new Flight { Number = "BA1234" });

        Assert.True(id > 0);
        Assert.Single(db.Flights);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnEntity()
    {
        using var db = TestDbContextFactory.Create();
        db.Flights.Add(new Flight { Number = "BA0001" });
        await db.SaveChangesAsync();

        var svc = new FlightsService(db);
        var flight = await svc.GetByIdAsync(1);

        Assert.NotNull(flight);
        Assert.Equal("BA0001", flight.Number);
    }

    [Fact]
    public async Task GetByIdAsync_InvalidId_ShouldThrow()
    {
        using var db = TestDbContextFactory.Create();
        var svc = new FlightsService(db);

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => svc.GetByIdAsync(0));
    }

    [Fact]
    public void GetAll_ShouldReturnQueryable()
    {
        using var db = TestDbContextFactory.Create();
        db.Flights.Add(new Flight { Number = "F1" });
        db.Flights.Add(new Flight { Number = "F2" });
        db.SaveChanges();

        var svc = new FlightsService(db);
        var all = svc.GetAll().ToList();

        Assert.Equal(2, all.Count);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldSetIsDeleted()
    {
        using var db = TestDbContextFactory.Create();
        db.Flights.Add(new Flight { Number = "DEL1" });
        await db.SaveChangesAsync();

        var svc = new FlightsService(db);
        var deleted = await svc.SoftDeleteAsync(1);

        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
    }

    [Fact]
    public async Task UpdateAsync_ShouldApplyChanges()
    {
        using var db = TestDbContextFactory.Create();
        db.Flights.Add(new Flight { Number = "OLD" });
        await db.SaveChangesAsync();

        var svc = new FlightsService(db);
        var updated = await svc.UpdateAsync(1, f => f.Number = "NEW");

        Assert.NotNull(updated);
        Assert.Equal("NEW", updated.Number);
    }
}
