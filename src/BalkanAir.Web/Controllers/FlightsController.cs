namespace BalkanAir.Web.Controllers;

using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FlightsController(ILegInstancesService legInstances) : Controller
{
    private const int PageSize = 20;

    public async Task<IActionResult> Index(int page = 1)
    {
        var query = legInstances.GetAll()
            .Where(l => !l.IsDeleted)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Origin!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Destination!)
            .Include(l => l.FlightStatus)
            .OrderByDescending(l => l.DepartureDateTime);

        var totalCount = await query.CountAsync();
        var flights = await query
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize);
        ViewBag.TotalFlights = totalCount;
        return View(flights);
    }

    public async Task<IActionResult> Details(int id)
    {
        var flight = await legInstances.GetAll()
            .Where(l => l.Id == id)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Origin!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Destination!)
            .Include(l => l.FlightStatus)
            .Include(l => l.Aircraft!)
                .ThenInclude(a => a.AircraftManufacturer!)
            .FirstOrDefaultAsync();

        if (flight is null) return NotFound();
        return View(flight);
    }
}
