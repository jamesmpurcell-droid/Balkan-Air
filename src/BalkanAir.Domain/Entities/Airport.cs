namespace BalkanAir.Domain.Entities;

public class Airport
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Abbreviation { get; set; }
    public bool IsDeleted { get; set; }

    public int CountryId { get; set; }
    public Country? Country { get; set; }

    public ICollection<Route> Origins { get; set; } = [];
    public ICollection<Route> Destinations { get; set; } = [];
}
