namespace BalkanAir.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsDeleted { get; set; }

    public ICollection<News> News { get; set; } = [];
}
