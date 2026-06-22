namespace BalkanAir.Web.Controllers;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Authorize(Roles = UserRoles.Administrator)]
public class AdministrationController(
    IAircraftsService aircrafts,
    IAircraftManufacturersService manufacturers,
    IAirportsService airports,
    IBookingsService bookings,
    ICategoriesService categories,
    ICountriesService countries,
    IFaresService fares,
    IFlightsService flights,
    IFlightLegsService flightLegs,
    ILegInstancesService legInstances,
    INewsService news,
    IRoutesService routes,
    ISeatsService seats,
    ITravelClassesService travelClasses,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole> roleManager) : Controller
{
    public async Task<IActionResult> Index()
    {
        ViewBag.AircraftCount = await aircrafts.GetAll().CountAsync();
        ViewBag.AirportCount = await airports.GetAll().CountAsync();
        ViewBag.FlightCount = await flights.GetAll().CountAsync();
        ViewBag.BookingCount = await bookings.GetAll().CountAsync();
        ViewBag.NewsCount = await news.GetAll().CountAsync();
        ViewBag.UserCount = userManager.Users.Count();
        return View();
    }

    public async Task<IActionResult> Aircraft() =>
        View(await aircrafts.GetAll().Include(a => a.AircraftManufacturer).ToListAsync());

    public async Task<IActionResult> Airports() =>
        View(await airports.GetAll().Include(a => a.Country).ToListAsync());

    public async Task<IActionResult> Flights() =>
        View(await flights.GetAll().ToListAsync());

    public async Task<IActionResult> FlightLegs() =>
        View(await flightLegs.GetAll().Include(fl => fl.Route).ToListAsync());

    public async Task<IActionResult> LegInstances() =>
        View(await legInstances.GetAll()
            .Include(l => l.FlightLeg)
            .OrderByDescending(l => l.DepartureDateTime)
            .ToListAsync());

    public async Task<IActionResult> Bookings() =>
        View(await bookings.GetAll()
            .Include(b => b.LegInstance)
            .OrderByDescending(b => b.DateOfBooking)
            .ToListAsync());

    public async Task<IActionResult> News() =>
        View(await news.GetAll().OrderByDescending(n => n.DateCreated).ToListAsync());

    public async Task<IActionResult> Routes() =>
        View(await routes.GetAll()
            .Include(r => r.Origin)
            .Include(r => r.Destination)
            .ToListAsync());

    public async Task<IActionResult> Categories() =>
        View(await categories.GetAll().ToListAsync());

    public async Task<IActionResult> Countries() =>
        View(await countries.GetAll().ToListAsync());

    public async Task<IActionResult> TravelClasses() =>
        View(await travelClasses.GetAll().ToListAsync());

    public async Task<IActionResult> Fares() =>
        View(await fares.GetAll().Include(f => f.Route).ToListAsync());

    public async Task<IActionResult> Seats() =>
        View(await seats.GetAll().ToListAsync());

    public async Task<IActionResult> Manufacturers() =>
        View(await manufacturers.GetAll().ToListAsync());

    public async Task<IActionResult> Users() =>
        View(await userManager.Users.ToListAsync());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAircraft(int id)
    {
        await aircrafts.SoftDeleteAsync(id);
        return RedirectToAction("Aircraft");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAirport(int id)
    {
        await airports.SoftDeleteAsync(id);
        return RedirectToAction("Airports");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        await flights.SoftDeleteAsync(id);
        return RedirectToAction("Flights");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        await bookings.SoftDeleteAsync(id);
        return RedirectToAction("Bookings");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNews(int id)
    {
        await news.SoftDeleteAsync(id);
        return RedirectToAction("News");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SeedRoles()
    {
        foreach (var role in new[] { UserRoles.Administrator, UserRoles.User })
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        TempData["Success"] = "Roles seeded successfully.";
        return RedirectToAction("Index");
    }
}
