namespace BalkanAir.Domain.Entities;

public class Country
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<Airport> Airports { get; set; } = [];
}
