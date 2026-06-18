namespace BalkanAir.Domain.Entities;

public class AircraftManufacturer
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Aircraft> Aircrafts { get; set; } = [];
}
