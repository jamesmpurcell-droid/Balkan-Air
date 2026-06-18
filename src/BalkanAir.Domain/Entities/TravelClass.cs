namespace BalkanAir.Domain.Entities;

using BalkanAir.Domain.Enums;

public class TravelClass
{
    public int Id { get; set; }
    public TravelClassType Type { get; set; }
    public required string Meal { get; set; }
    public bool PriorityBoarding { get; set; }
    public bool ReservedSeat { get; set; } = true;
    public bool EarnMiles { get; set; }
    public int NumberOfRows { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }

    public int AircraftId { get; set; }
    public Aircraft? Aircraft { get; set; }
}
