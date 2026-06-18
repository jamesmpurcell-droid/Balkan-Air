namespace BalkanAir.Domain.Entities;

public class Comment
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public DateTime DateOfComment { get; set; }
    public bool IsDeleted { get; set; }

    public string? UserId { get; set; }
    public User? User { get; set; }

    public int NewsId { get; set; }
    public News? News { get; set; }
}
