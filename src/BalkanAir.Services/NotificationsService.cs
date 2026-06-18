namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class NotificationsService(BalkanAirDbContext db) : CrudService<Notification>(db), INotificationsService
{
    protected override DbSet<Notification> Set => Db.Notifications;
    protected override void MarkDeleted(Notification e) => e.IsDeleted = true;
}
