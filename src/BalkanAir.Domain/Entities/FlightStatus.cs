namespace BalkanAir.Domain.Entities;

public class FlightStatus
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<LegInstance> LegInstances { get; set; } = [];
}
