namespace BalkanAir.Domain.Entities;

public class Fare
{
    public int Id { get; set; }
    public decimal Price { get; set; }
    public bool IsDeleted { get; set; }

    public int RouteId { get; set; }
    public Route? Route { get; set; }
}
