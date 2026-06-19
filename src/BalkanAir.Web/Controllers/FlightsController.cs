namespace BalkanAir.Web.Controllers;

using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class FlightsController(ILegInstancesService legInstances) : Controller
{
    public async Task<IActionResult> Index()
    {
        var flights = await legInstances.GetAll()
            .OrderByDescending(l => l.DepartureDateTime)
            .ToListAsync();
        return View(flights);
    }

    public async Task<IActionResult> Details(int id)
    {
        var flight = await legInstances.GetByIdAsync(id);
        if (flight is null) return NotFound();
        return View(flight);
    }
}
