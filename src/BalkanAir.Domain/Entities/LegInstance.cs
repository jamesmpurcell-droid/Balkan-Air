namespace BalkanAir.Domain.Entities;

public class LegInstance
{
    public int Id { get; set; }
    public DateTime DepartureDateTime { get; set; }
    public DateTime ArrivalDateTime { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }

    public int FlightLegId { get; set; }
    public FlightLeg? FlightLeg { get; set; }

    public int FlightStatusId { get; set; }
    public FlightStatus? FlightStatus { get; set; }

    public int AircraftId { get; set; }
    public Aircraft? Aircraft { get; set; }

    public ICollection<Seat> Seats { get; set; } = [];
    public ICollection<Booking> Bookings { get; set; } = [];

    public TimeSpan Duration => ArrivalDateTime - DepartureDateTime;
}
