namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;

public class Notification
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public NotificationType Type { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<UserNotification> UserNotifications { get; set; } = [];
}
