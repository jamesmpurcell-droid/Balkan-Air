namespace BalkanAir.Domain.Entities;

using BalkanAir.Common;

public class Aircraft
{
    public int Id { get; set; }
    public required string Model { get; set; }
    public int TotalSeats { get; set; } = ValidationConstants.AircraftMaxSeats;
    public bool IsDeleted { get; set; }

    public int AircraftManufacturerId { get; set; }
    public AircraftManufacturer? AircraftManufacturer { get; set; }

    public ICollection<TravelClass> TravelClasses { get; set; } = [];
    public ICollection<LegInstance> LegInstances { get; set; } = [];

    public decimal CheapestTravelClassPrice =>
        TravelClasses.Count == 0 ? 0m : TravelClasses.Min(t => t.Price);
}
