namespace BalkanAir.Services.Tests;

using BalkanAir.Domain.Entities;

public class UsersServiceTests
{
    private static ApplicationUser MakeUser(string id = "u1") => new()
    {
        Id = id,
        Email = "test@balkanair.com",
        UserName = "test@balkanair.com"
    };

    [Fact]
    public async Task AddAsync_ShouldPersistUser()
    {
        using var db = TestDbContextFactory.Create();
        var svc = new UsersService(db);

        var returnedId = await svc.AddAsync(MakeUser());

        Assert.Equal("u1", returnedId);
        Assert.Single(db.Users);
    }

    [Fact]
    public async Task GetByEmailAsync_ShouldReturnUser()
    {
        using var db = TestDbContextFactory.Create();
        db.Users.Add(MakeUser());
        await db.SaveChangesAsync();

        var svc = new UsersService(db);
        var user = await svc.GetByEmailAsync("test@balkanair.com");

        Assert.NotNull(user);
        Assert.Equal("u1", user.Id);
    }

    [Fact]
    public async Task SoftDeleteAsync_ShouldSetDeletedOnTimestamp()
    {
        using var db = TestDbContextFactory.Create();
        db.Users.Add(MakeUser());
        await db.SaveChangesAsync();

        var svc = new UsersService(db);
        var deleted = await svc.SoftDeleteAsync("u1");

        Assert.NotNull(deleted);
        Assert.True(deleted.IsDeleted);
        Assert.NotNull(deleted.DeletedOn);
    }

    [Fact]
    public async Task SetLastLoginAsync_ShouldUpdateLastLogin()
    {
        using var db = TestDbContextFactory.Create();
        db.Users.Add(MakeUser());
        await db.SaveChangesAsync();

        var svc = new UsersService(db);
        var loginTime = new DateTime(2024, 6, 15, 10, 0, 0, DateTimeKind.Utc);
        await svc.SetLastLoginAsync("test@balkanair.com", loginTime);

        var user = await db.Users.FindAsync("u1");
        Assert.NotNull(user);
        Assert.Equal(loginTime, user.LastLogin);
    }
}
