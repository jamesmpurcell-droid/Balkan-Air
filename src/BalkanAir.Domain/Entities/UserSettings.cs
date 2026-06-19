namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;

public class UserSettings
{
    public byte[]? ProfilePicture { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? IdentityDocumentNumber { get; set; }
    public string? Nationality { get; set; }
    public string? FullAddress { get; set; }
    public bool ReceiveEmailWhenNewNews { get; set; } = true;
    public bool ReceiveEmailWhenNewFlight { get; set; } = true;
    public bool ReceiveNotificationWhenNewNews { get; set; } = true;
    public bool ReceiveNotificationWhenNewFlight { get; set; } = true;
    public DateTime LastLogin { get; set; }
    public DateTime LastLogout { get; set; }

    public ICollection<Booking> Bookings { get; set; } = [];
}
