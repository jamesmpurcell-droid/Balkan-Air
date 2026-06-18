namespace BalkanAir.Services.Tests;

using BalkanAir.Data;
using Microsoft.EntityFrameworkCore;

internal static class TestDbContextFactory
{
    public static BalkanAirDbContext Create(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<BalkanAirDbContext>()
            .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
            .Options;

        return new BalkanAirDbContext(options);
    }
}
