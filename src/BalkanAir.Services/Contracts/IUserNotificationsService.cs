namespace BalkanAir.Services.Contracts;

using BalkanAir.Domain.Entities;

public interface IUserNotificationsService
{
    Task SendNotificationAsync(int notificationId, string userId);
    Task SendNotificationAsync(int notificationId, IEnumerable<string> userIds);
    Task SetNotificationAsReadAsync(int notificationId, string userId);
    Task SetAllNotificationsAsReadAsync(string userId);
    Task<UserNotification?> GetByIdAsync(int id);
    IQueryable<UserNotification> GetAll();
}
