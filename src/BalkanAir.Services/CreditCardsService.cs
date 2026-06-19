namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class CreditCardsService(BalkanAirDbContext db) : CrudService<CreditCard>(db), ICreditCardsService
{
    protected override DbSet<CreditCard> Set => Db.CreditCards;
    protected override void MarkDeleted(CreditCard e) => e.IsDeleted = true;
}
