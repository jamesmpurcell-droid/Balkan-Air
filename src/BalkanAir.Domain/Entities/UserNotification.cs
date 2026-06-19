namespace BalkanAir.Domain.Entities;

public class UserNotification
{
    public int Id { get; set; }
    public DateTime DateReceived { get; set; }
    public bool IsRead { get; set; }
    public DateTime? DateRead { get; set; }

    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public int NotificationId { get; set; }
    public Notification? Notification { get; set; }
}
