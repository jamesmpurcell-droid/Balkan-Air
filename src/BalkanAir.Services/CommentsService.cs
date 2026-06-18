namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class CommentsService(BalkanAirDbContext db) : CrudService<Comment>(db), ICommentsService
{
    protected override DbSet<Comment> Set => Db.Comments;
    protected override void MarkDeleted(Comment e) => e.IsDeleted = true;
}
