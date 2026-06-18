namespace BalkanAir.Domain.Entities;

public class News
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime DateCreated { get; set; }
    public byte[]? HeaderImage { get; set; }
    public bool IsDeleted { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }

    public ICollection<Comment> Comments { get; set; } = [];
}
