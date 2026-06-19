namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? IdentityDocumentNumber { get; set; }
    public string? Nationality { get; set; }
    public string? FullAddress { get; set; }
    public byte[]? ProfilePicture { get; set; }

    public bool ReceiveEmailWhenNewNews { get; set; } = true;
    public bool ReceiveEmailWhenNewFlight { get; set; } = true;
    public bool ReceiveNotificationWhenNewNews { get; set; } = true;
    public bool ReceiveNotificationWhenNewFlight { get; set; } = true;

    public DateTime? LastLogin { get; set; }
    public DateTime? LastLogout { get; set; }
    public bool DoesAdminForcedLogoff { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];
    public ICollection<CreditCard> CreditCards { get; set; } = [];
    public ICollection<UserNotification> UserNotifications { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}
