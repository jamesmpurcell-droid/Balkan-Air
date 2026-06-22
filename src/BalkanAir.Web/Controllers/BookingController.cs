namespace BalkanAir.Web.Controllers;

using BalkanAir.Domain.Entities;
using BalkanAir.Domain.Enums;
using BalkanAir.Services.Contracts;
using BalkanAir.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class BookingController(
    IAirportsService airports,
    ILegInstancesService legInstances,
    ITravelClassesService travelClasses,
    IBookingsService bookings,
    IFaresService fares,
    UserManager<ApplicationUser> userManager) : Controller
{
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Search()
    {
        await PopulateAirportsAsync();
        return View(new BookingSearchViewModel());
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Search(BookingSearchViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAirportsAsync();
            return View(model);
        }

        var results = await legInstances.GetAll()
            .Where(l => l.FlightLeg != null
                && l.FlightLeg.Route != null
                && l.FlightLeg.Route.OriginId == model.DepartureAirportId
                && l.FlightLeg.Route.DestinationId == model.DestinationAirportId
                && l.DepartureDateTime.Date == model.DepartureDate!.Value.Date
                && !l.IsDeleted)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Origin!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Destination!)
            .OrderBy(l => l.DepartureDateTime)
            .ToListAsync();

        ViewBag.SearchResults = results;
        await PopulateAirportsAsync();
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Confirm(int legInstanceId)
    {
        var flight = await legInstances.GetAll()
            .Where(l => l.Id == legInstanceId)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Origin!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Destination!)
            .FirstOrDefaultAsync();

        if (flight is null) return NotFound();

        ViewBag.Flight = flight;
        ViewBag.TravelClasses = new SelectList(
            await travelClasses.GetAll().Where(t => !t.IsDeleted).ToListAsync(),
            "Id", "Type");

        var fare = flight.FlightLeg?.RouteId is int routeId
            ? await fares.GetAll().FirstOrDefaultAsync(f => f.RouteId == routeId && !f.IsDeleted)
            : null;
        ViewBag.FarePrice = fare?.Price ?? flight.Price;

        return View(new BookingConfirmViewModel { LegInstanceId = legInstanceId, SeatNumber = "A" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Confirm(BookingConfirmViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Flight = await legInstances.GetByIdAsync(model.LegInstanceId);
            ViewBag.TravelClasses = new SelectList(
                await travelClasses.GetAll().Where(t => !t.IsDeleted).ToListAsync(),
                "Id", "Type");
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var legInstance = await legInstances.GetByIdAsync(model.LegInstanceId);
        var routeId = legInstance?.FlightLeg?.RouteId;
        var fare = routeId is not null
            ? await fares.GetAll().FirstOrDefaultAsync(f => f.RouteId == routeId && !f.IsDeleted)
            : null;

        var booking = new Booking
        {
            ConfirmationCode = Guid.NewGuid().ToString("N")[..6].ToUpperInvariant(),
            DateOfBooking = DateTime.UtcNow,
            Row = model.Row,
            SeatNumber = model.SeatNumber,
            TotalPrice = fare?.Price ?? 0m,
            TravelClassId = model.TravelClassId,
            Status = BookingStatus.Confirmed,
            UserId = user.Id,
            LegInstanceId = model.LegInstanceId,
        };

        var id = await bookings.AddAsync(booking);
        return RedirectToAction("Confirmation", new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Confirmation(int id)
    {
        var booking = await bookings.GetAll()
            .Where(b => b.Id == id)
            .Include(b => b.LegInstance!)
                .ThenInclude(l => l.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Origin!)
            .Include(b => b.LegInstance!)
                .ThenInclude(l => l.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Destination!)
            .FirstOrDefaultAsync();

        if (booking is null) return NotFound();
        return View(booking);
    }

    [HttpGet]
    public async Task<IActionResult> MyBookings()
    {
        var user = await userManager.GetUserAsync(User);
        if (user is null) return Challenge();

        var userBookings = await bookings.GetAll()
            .Where(b => b.UserId == user.Id && !b.IsDeleted)
            .Include(b => b.LegInstance!)
                .ThenInclude(l => l.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Origin!)
            .Include(b => b.LegInstance!)
                .ThenInclude(l => l.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Destination!)
            .OrderByDescending(b => b.DateOfBooking)
            .ToListAsync();

        return View(userBookings);
    }

    private async Task PopulateAirportsAsync()
    {
        var airportList = await airports.GetAll()
            .Where(a => !a.IsDeleted)
            .OrderBy(a => a.Name)
            .ToListAsync();
        ViewBag.Airports = new SelectList(airportList, "Id", "Name");
    }
}
