namespace BalkanAir.Domain.Entities;

public class Route
{
    public int Id { get; set; }

    public int OriginId { get; set; }
    public Airport? Origin { get; set; }

    public int DestinationId { get; set; }
    public Airport? Destination { get; set; }

    public double DistanceInKm { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Fare> Fares { get; set; } = [];
    public ICollection<FlightLeg> FlightLegs { get; set; } = [];
}
