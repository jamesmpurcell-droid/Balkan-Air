namespace BalkanAir.Domain.Entities;

public class CreditCard
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public required string NameOnCard { get; set; }
    public int ExpirationMonth { get; set; }
    public int ExpirationYear { get; set; }
    public required string CvvNumber { get; set; }
    public bool IsDeleted { get; set; }

    public required string UserId { get; set; }
    public ApplicationUser? User { get; set; }
}
