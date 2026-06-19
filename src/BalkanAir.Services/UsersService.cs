namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class UsersService(BalkanAirDbContext db) : IUsersService
{
    public async Task<string> AddAsync(ApplicationUser entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        db.Users.Add(entity);
        await db.SaveChangesAsync();
        return entity.Id;
    }

    public async Task<ApplicationUser?> GetByIdAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        return await db.Users.FindAsync(id);
    }

    public async Task<ApplicationUser?> GetByEmailAsync(string email)
    {
        ArgumentException.ThrowIfNullOrEmpty(email);
        return await db.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public IQueryable<ApplicationUser> GetAll() => db.Users.AsQueryable();

    public async Task<ApplicationUser?> UpdateAsync(string id, Action<ApplicationUser> applyUpdates)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        ArgumentNullException.ThrowIfNull(applyUpdates);

        var user = await db.Users.FindAsync(id);
        if (user is null) return null;

        applyUpdates(user);
        await db.SaveChangesAsync();
        return user;
    }

    public async Task<ApplicationUser?> SoftDeleteAsync(string id)
    {
        ArgumentException.ThrowIfNullOrEmpty(id);
        var user = await db.Users.FindAsync(id);
        if (user is null) return null;

        user.IsDeleted = true;
        user.DeletedOn = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return user;
    }

    public async Task UploadProfilePictureAsync(string userId, byte[] image)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        ArgumentNullException.ThrowIfNull(image);

        var user = await db.Users.FindAsync(userId);
        if (user is not null)
        {
            user.ProfilePicture = image;
            await db.SaveChangesAsync();
        }
    }

    public async Task SetLastLoginAsync(string userEmail, DateTime dateTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(userEmail);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user is not null)
        {
            user.LastLogin = dateTime;
            await db.SaveChangesAsync();
        }
    }

    public async Task SetLastLogoutAsync(string userEmail, DateTime dateTime)
    {
        ArgumentException.ThrowIfNullOrEmpty(userEmail);
        var user = await db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user is not null)
        {
            user.LastLogout = dateTime;
            await db.SaveChangesAsync();
        }
    }

    public async Task SetLogoffForUserAsync(string userId, bool logoff)
    {
        ArgumentException.ThrowIfNullOrEmpty(userId);
        var user = await db.Users.FindAsync(userId);
        if (user is not null)
        {
            user.DoesAdminForcedLogoff = logoff;
            await db.SaveChangesAsync();
        }
    }
}
