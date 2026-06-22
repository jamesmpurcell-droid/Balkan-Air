namespace BalkanAir.Web.Controllers;

using BalkanAir.Common;
using BalkanAir.Domain.Entities;
using BalkanAir.Services.Contracts;
using BalkanAir.Web.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

    // ── Country CRUD ────────────────────────────────────────────

    [HttpGet]
    public IActionResult CreateCountry() => View(new CountryFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCountry(CountryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await countries.AddAsync(new Country
        {
            Name = model.Name,
            Abbreviation = model.Abbreviation
        });

        TempData["Success"] = $"Country '{model.Name}' created.";
        return RedirectToAction("Countries");
    }

    [HttpGet]
    public async Task<IActionResult> EditCountry(int id)
    {
        var entity = await countries.GetByIdAsync(id);
        if (entity is null) return NotFound();

        return View(new CountryFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Abbreviation = entity.Abbreviation
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCountry(CountryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await countries.UpdateAsync(model.Id, c =>
        {
            c.Name = model.Name;
            c.Abbreviation = model.Abbreviation;
        });

        TempData["Success"] = $"Country '{model.Name}' updated.";
        return RedirectToAction("Countries");
    }

    // ── Airport CRUD ────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateAirport()
    {
        await PopulateCountriesDropdown();
        return View(new AirportFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAirport(AirportFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCountriesDropdown();
            return View(model);
        }

        await airports.AddAsync(new Airport
        {
            Name = model.Name,
            Abbreviation = model.Abbreviation,
            CountryId = model.CountryId
        });

        TempData["Success"] = $"Airport '{model.Name}' created.";
        return RedirectToAction("Airports");
    }

    [HttpGet]
    public async Task<IActionResult> EditAirport(int id)
    {
        var entity = await airports.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await PopulateCountriesDropdown();
        return View(new AirportFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name,
            Abbreviation = entity.Abbreviation,
            CountryId = entity.CountryId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAirport(AirportFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCountriesDropdown();
            return View(model);
        }

        await airports.UpdateAsync(model.Id, a =>
        {
            a.Name = model.Name;
            a.Abbreviation = model.Abbreviation;
            a.CountryId = model.CountryId;
        });

        TempData["Success"] = $"Airport '{model.Name}' updated.";
        return RedirectToAction("Airports");
    }

    // ── Route CRUD ──────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateRoute()
    {
        await PopulateAirportsDropdown();
        return View(new RouteFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateRoute(RouteFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAirportsDropdown();
            return View(model);
        }

        var route = new Route
        {
            OriginId = model.OriginId,
            DestinationId = model.DestinationId,
            DistanceInKm = model.DistanceInKm
        };
        var routeId = await routes.AddAsync(route);

        if (model.FarePrice > 0)
        {
            await fares.AddAsync(new Fare { RouteId = routeId, Price = model.FarePrice });
        }

        TempData["Success"] = "Route created.";
        return RedirectToAction("Routes");
    }

    [HttpGet]
    public async Task<IActionResult> EditRoute(int id)
    {
        var entity = await routes.GetByIdAsync(id);
        if (entity is null) return NotFound();

        var fare = await fares.GetAll().FirstOrDefaultAsync(f => f.RouteId == id && !f.IsDeleted);
        await PopulateAirportsDropdown();
        return View(new RouteFormViewModel
        {
            Id = entity.Id,
            OriginId = entity.OriginId,
            DestinationId = entity.DestinationId,
            DistanceInKm = entity.DistanceInKm,
            FarePrice = fare?.Price ?? 0m
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditRoute(RouteFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAirportsDropdown();
            return View(model);
        }

        await routes.UpdateAsync(model.Id, r =>
        {
            r.OriginId = model.OriginId;
            r.DestinationId = model.DestinationId;
            r.DistanceInKm = model.DistanceInKm;
        });

        var fare = await fares.GetAll().FirstOrDefaultAsync(f => f.RouteId == model.Id && !f.IsDeleted);
        if (fare is not null)
        {
            await fares.UpdateAsync(fare.Id, f => f.Price = model.FarePrice);
        }
        else if (model.FarePrice > 0)
        {
            await fares.AddAsync(new Fare { RouteId = model.Id, Price = model.FarePrice });
        }

        TempData["Success"] = "Route updated.";
        return RedirectToAction("Routes");
    }

    // ── News CRUD ───────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateNews()
    {
        await PopulateCategoriesDropdown();
        return View(new NewsFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateNews(NewsFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesDropdown();
            return View(model);
        }

        await news.AddAsync(new News
        {
            Title = model.Title,
            Content = model.Content,
            CategoryId = model.CategoryId,
            DateCreated = DateTime.UtcNow
        });

        TempData["Success"] = $"News article '{model.Title}' created.";
        return RedirectToAction("News");
    }

    [HttpGet]
    public async Task<IActionResult> EditNews(int id)
    {
        var entity = await news.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await PopulateCategoriesDropdown();
        return View(new NewsFormViewModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Content = entity.Content,
            CategoryId = entity.CategoryId
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditNews(NewsFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateCategoriesDropdown();
            return View(model);
        }

        await news.UpdateAsync(model.Id, n =>
        {
            n.Title = model.Title;
            n.Content = model.Content;
            n.CategoryId = model.CategoryId;
        });

        TempData["Success"] = $"News article '{model.Title}' updated.";
        return RedirectToAction("News");
    }

    // ── Dropdown helpers ────────────────────────────────────────

    private async Task PopulateCountriesDropdown()
    {
        ViewBag.Countries = new SelectList(
            await countries.GetAll().Where(c => !c.IsDeleted).OrderBy(c => c.Name).ToListAsync(),
            "Id", "Name");
    }

    private async Task PopulateAirportsDropdown()
    {
        ViewBag.Airports = new SelectList(
            await airports.GetAll().Where(a => !a.IsDeleted).OrderBy(a => a.Name).ToListAsync(),
            "Id", "Name");
    }

    private async Task PopulateCategoriesDropdown()
    {
        ViewBag.Categories = new SelectList(
            await categories.GetAll().Where(c => !c.IsDeleted).ToListAsync(),
            "Id", "Name");
    }
}
