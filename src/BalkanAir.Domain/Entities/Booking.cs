namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;

public class Booking
{
    public int Id { get; set; }
    public required string ConfirmationCode { get; set; }
    public DateTime DateOfBooking { get; set; }
    public int Row { get; set; }
    public required string SeatNumber { get; set; }
    public decimal TotalPrice { get; set; }
    public int TravelClassId { get; set; }
    public BookingStatus Status { get; set; }
    public bool IsDeleted { get; set; }

    public required string UserId { get; set; }
    public User? User { get; set; }

    public int LegInstanceId { get; set; }
    public LegInstance? LegInstance { get; set; }

    public ICollection<Baggage> Baggage { get; set; } = [];
}
