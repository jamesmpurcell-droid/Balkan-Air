namespace BalkanAir.Domain.Entities;

public class Flight
{
    public int Id { get; set; }
    public required string Number { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<FlightLeg> FlightLegs { get; set; } = [];
}
