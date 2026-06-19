namespace BalkanAir.Domain.Entities;

public class Seat
{
    public int Id { get; set; }
    public int Row { get; set; }
    public required string Number { get; set; }
    public int TravelClassId { get; set; }
    public bool IsReserved { get; set; }
    public bool IsDeleted { get; set; }

    public int LegInstanceId { get; set; }
    public LegInstance? LegInstance { get; set; }
}
