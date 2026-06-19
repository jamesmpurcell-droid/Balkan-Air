namespace BalkanAir.Services;

using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.EntityFrameworkCore;

public sealed class CountriesService(BalkanAirDbContext db) : CrudService<Country>(db), ICountriesService
{
    protected override DbSet<Country> Set => Db.Countries;
    protected override void MarkDeleted(Country e) => e.IsDeleted = true;
}
