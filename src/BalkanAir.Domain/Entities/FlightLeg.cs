namespace BalkanAir.Domain.Entities;

public class FlightLeg
{
    public int Id { get; set; }

    public int DepartureAirportId { get; set; }
    public DateTime ScheduledDepartureDateTime { get; set; }

    public int ArrivalAirportId { get; set; }
    public DateTime ScheduledArrivalDateTime { get; set; }

    public bool IsDeleted { get; set; }

    public int FlightId { get; set; }
    public Flight? Flight { get; set; }

    public int RouteId { get; set; }
    public Route? Route { get; set; }

    public ICollection<LegInstance> LegInstances { get; set; } = [];
}
