namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class UserNotificationsService(BalkanAirDbContext db) : IUserNotificationsService
{
    public async Task SendNotificationAsync(int notificationId, string userId)
    {
        db.UserNotifications.Add(new UserNotification
        {
            NotificationId = notificationId,
            UserId = userId,
            DateReceived = DateTime.UtcNow
        });
        await db.SaveChangesAsync();
    }

    public async Task SendNotificationAsync(int notificationId, IEnumerable<string> userIds)
    {
        ArgumentNullException.ThrowIfNull(userIds);

        foreach (var userId in userIds)
        {
            db.UserNotifications.Add(new UserNotification
            {
                NotificationId = notificationId,
                UserId = userId,
                DateReceived = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }

    public async Task SetNotificationAsReadAsync(int notificationId, string userId)
    {
        var un = await db.UserNotifications
            .FirstOrDefaultAsync(n => n.NotificationId == notificationId && n.UserId == userId);

        if (un is not null)
        {
            un.IsRead = true;
            un.DateRead = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }

    public async Task SetAllNotificationsAsReadAsync(string userId)
    {
        var unread = await db.UserNotifications
            .Where(n => n.UserId == userId && !n.IsRead)
            .ToListAsync();

        foreach (var un in unread)
        {
            un.IsRead = true;
            un.DateRead = DateTime.UtcNow;
        }

        await db.SaveChangesAsync();
    }

    public async Task<UserNotification?> GetByIdAsync(int id)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        return await db.UserNotifications.FindAsync(id);
    }

    public IQueryable<UserNotification> GetAll() => db.UserNotifications.AsQueryable();
}
