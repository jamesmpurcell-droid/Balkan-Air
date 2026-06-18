namespace BalkanAir.Domain.Entities;

/// <summary>
/// Domain user. In PR7 (ASP.NET Core Identity) this will be extended or replaced
/// by an ApplicationUser that inherits from IdentityUser.
/// </summary>
public class User
{
    public required string Id { get; set; }
    public required string Email { get; set; }
    public string? UserName { get; set; }
    public string? PhoneNumber { get; set; }

    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public bool IsDeleted { get; set; }
    public bool DoesAdminForcedLogoff { get; set; }

    public UserSettings UserSettings { get; set; } = new();

    public ICollection<CreditCard> CreditCards { get; set; } = [];
    public ICollection<UserNotification> UserNotifications { get; set; } = [];
    public ICollection<Comment> Comments { get; set; } = [];
}
