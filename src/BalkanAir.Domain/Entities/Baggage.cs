namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;

public class Baggage
{
    public int Id { get; set; }
    public BaggageType Type { get; set; }
    public int MaxKilograms { get; set; }
    public string? Size { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }

    public int BookingId { get; set; }
    public Booking? Booking { get; set; }
}
