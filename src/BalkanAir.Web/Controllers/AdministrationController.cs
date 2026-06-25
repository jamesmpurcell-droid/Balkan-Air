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
    // ── Dashboard ────────────────────────────────────────────────

    public async Task<IActionResult> Index()
    {
        ViewBag.AircraftCount = await aircrafts.GetAll().CountAsync();
        ViewBag.AirportCount = await airports.GetAll().CountAsync();
        ViewBag.FlightCount = await flights.GetAll().CountAsync();
        ViewBag.BookingCount = await bookings.GetAll().CountAsync();
        ViewBag.NewsCount = await news.GetAll().CountAsync();
        ViewBag.UserCount = userManager.Users.Count();
        ViewBag.RouteCount = await routes.GetAll().CountAsync();
        ViewBag.ManufacturerCount = await manufacturers.GetAll().CountAsync();
        ViewBag.CategoryCount = await categories.GetAll().CountAsync();
        ViewBag.TravelClassCount = await travelClasses.GetAll().CountAsync();
        ViewBag.FareCount = await fares.GetAll().CountAsync();
        return View();
    }

    // ── List views ───────────────────────────────────────────────

    public async Task<IActionResult> Aircraft() =>
        View(await aircrafts.GetAll().Include(a => a.AircraftManufacturer).ToListAsync());

    public async Task<IActionResult> Airports() =>
        View(await airports.GetAll().Include(a => a.Country).ToListAsync());

    public async Task<IActionResult> Flights() =>
        View(await flights.GetAll().Include(f => f.FlightLegs).ToListAsync());

    public async Task<IActionResult> FlightLegs() =>
        View(await flightLegs.GetAll()
            .Include(fl => fl.Flight)
            .Include(fl => fl.Route!)
                .ThenInclude(r => r.Origin!)
            .Include(fl => fl.Route!)
                .ThenInclude(r => r.Destination!)
            .Include(fl => fl.LegInstances)
            .ToListAsync());

    public async Task<IActionResult> LegInstances() =>
        View(await legInstances.GetAll()
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Flight!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Origin!)
            .Include(l => l.FlightLeg!)
                .ThenInclude(fl => fl.Route!)
                    .ThenInclude(r => r.Destination!)
            .Include(l => l.Aircraft!)
                .ThenInclude(a => a.AircraftManufacturer!)
            .Include(l => l.Bookings)
            .OrderByDescending(l => l.DepartureDateTime)
            .ToListAsync());

    public async Task<IActionResult> Bookings() =>
        View(await bookings.GetAll()
            .Include(b => b.User)
            .Include(b => b.LegInstance!)
                .ThenInclude(li => li.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Origin!)
            .Include(b => b.LegInstance!)
                .ThenInclude(li => li.FlightLeg!)
                    .ThenInclude(fl => fl.Route!)
                        .ThenInclude(r => r.Destination!)
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
        View(await categories.GetAll().Include(c => c.News).ToListAsync());

    public async Task<IActionResult> Countries() =>
        View(await countries.GetAll().ToListAsync());

    public async Task<IActionResult> TravelClasses() =>
        View(await travelClasses.GetAll()
            .Include(tc => tc.Aircraft!)
                .ThenInclude(a => a.AircraftManufacturer!)
            .ToListAsync());

    public async Task<IActionResult> Fares() =>
        View(await fares.GetAll()
            .Include(f => f.Route!)
                .ThenInclude(r => r.Origin!)
            .Include(f => f.Route!)
                .ThenInclude(r => r.Destination!)
            .ToListAsync());

    public async Task<IActionResult> Seats() =>
        View(await seats.GetAll()
            .Include(s => s.LegInstance!)
                .ThenInclude(li => li.FlightLeg!)
                    .ThenInclude(fl => fl.Flight!)
            .ToListAsync());

    public async Task<IActionResult> Manufacturers() =>
        View(await manufacturers.GetAll().Include(m => m.Aircrafts).ToListAsync());

    public async Task<IActionResult> Users() =>
        View(await userManager.Users.ToListAsync());

    // ── Soft-delete actions ──────────────────────────────────────

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAircraft(int id)
    {
        await aircrafts.SoftDeleteAsync(id);
        return RedirectToAction("Aircraft");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteAirport(int id)
    {
        await airports.SoftDeleteAsync(id);
        return RedirectToAction("Airports");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFlight(int id)
    {
        await flights.SoftDeleteAsync(id);
        return RedirectToAction("Flights");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        await bookings.SoftDeleteAsync(id);
        return RedirectToAction("Bookings");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteNews(int id)
    {
        await news.SoftDeleteAsync(id);
        return RedirectToAction("News");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteManufacturer(int id)
    {
        await manufacturers.SoftDeleteAsync(id);
        return RedirectToAction("Manufacturers");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await categories.SoftDeleteAsync(id);
        return RedirectToAction("Categories");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteFare(int id)
    {
        await fares.SoftDeleteAsync(id);
        return RedirectToAction("Fares");
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTravelClass(int id)
    {
        await travelClasses.SoftDeleteAsync(id);
        return RedirectToAction("TravelClasses");
    }

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    [HttpPost, ValidateAntiForgeryToken]
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

    // ── Aircraft CRUD ──────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateAircraft()
    {
        await PopulateManufacturersDropdown();
        return View(new AircraftFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateAircraft(AircraftFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateManufacturersDropdown();
            return View(model);
        }

        await aircrafts.AddAsync(new Aircraft
        {
            Model = model.AircraftModel,
            AircraftManufacturerId = model.ManufacturerId,
            TotalSeats = model.TotalSeats
        });

        TempData["Success"] = $"Aircraft '{model.AircraftModel}' created.";
        return RedirectToAction("Aircraft");
    }

    [HttpGet]
    public async Task<IActionResult> EditAircraft(int id)
    {
        var entity = await aircrafts.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await PopulateManufacturersDropdown();
        return View(new AircraftFormViewModel
        {
            Id = entity.Id,
            AircraftModel = entity.Model,
            ManufacturerId = entity.AircraftManufacturerId,
            TotalSeats = entity.TotalSeats
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditAircraft(AircraftFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateManufacturersDropdown();
            return View(model);
        }

        await aircrafts.UpdateAsync(model.Id, a =>
        {
            a.Model = model.AircraftModel;
            a.AircraftManufacturerId = model.ManufacturerId;
            a.TotalSeats = model.TotalSeats;
        });

        TempData["Success"] = $"Aircraft '{model.AircraftModel}' updated.";
        return RedirectToAction("Aircraft");
    }

    // ── Manufacturer CRUD ───────────────────────────────────────

    [HttpGet]
    public IActionResult CreateManufacturer() => View(new ManufacturerFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateManufacturer(ManufacturerFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await manufacturers.AddAsync(new AircraftManufacturer { Name = model.Name });

        TempData["Success"] = $"Manufacturer '{model.Name}' created.";
        return RedirectToAction("Manufacturers");
    }

    [HttpGet]
    public async Task<IActionResult> EditManufacturer(int id)
    {
        var entity = await manufacturers.GetByIdAsync(id);
        if (entity is null) return NotFound();

        return View(new ManufacturerFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditManufacturer(ManufacturerFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await manufacturers.UpdateAsync(model.Id, m => m.Name = model.Name);

        TempData["Success"] = $"Manufacturer '{model.Name}' updated.";
        return RedirectToAction("Manufacturers");
    }

    // ── Category CRUD ───────────────────────────────────────────

    [HttpGet]
    public IActionResult CreateCategory() => View(new CategoryFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateCategory(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await categories.AddAsync(new Category { Name = model.Name });

        TempData["Success"] = $"Category '{model.Name}' created.";
        return RedirectToAction("Categories");
    }

    [HttpGet]
    public async Task<IActionResult> EditCategory(int id)
    {
        var entity = await categories.GetByIdAsync(id);
        if (entity is null) return NotFound();

        return View(new CategoryFormViewModel
        {
            Id = entity.Id,
            Name = entity.Name
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditCategory(CategoryFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await categories.UpdateAsync(model.Id, c => c.Name = model.Name);

        TempData["Success"] = $"Category '{model.Name}' updated.";
        return RedirectToAction("Categories");
    }

    // ── Fare CRUD ───────────────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateFare()
    {
        await PopulateRoutesDropdown();
        return View(new FareFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFare(FareFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRoutesDropdown();
            return View(model);
        }

        await fares.AddAsync(new Fare
        {
            RouteId = model.RouteId,
            Price = model.Price
        });

        TempData["Success"] = $"Fare created (${model.Price}).";
        return RedirectToAction("Fares");
    }

    [HttpGet]
    public async Task<IActionResult> EditFare(int id)
    {
        var entity = await fares.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await PopulateRoutesDropdown();
        return View(new FareFormViewModel
        {
            Id = entity.Id,
            RouteId = entity.RouteId,
            Price = entity.Price
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFare(FareFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateRoutesDropdown();
            return View(model);
        }

        await fares.UpdateAsync(model.Id, f =>
        {
            f.RouteId = model.RouteId;
            f.Price = model.Price;
        });

        TempData["Success"] = "Fare updated.";
        return RedirectToAction("Fares");
    }

    // ── Travel Class CRUD ───────────────────────────────────────

    [HttpGet]
    public async Task<IActionResult> CreateTravelClass()
    {
        await PopulateAircraftDropdown();
        return View(new TravelClassFormViewModel());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTravelClass(TravelClassFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAircraftDropdown();
            return View(model);
        }

        await travelClasses.AddAsync(new TravelClass
        {
            Type = model.Type,
            Meal = model.Meal,
            PriorityBoarding = model.PriorityBoarding,
            ReservedSeat = model.ReservedSeat,
            EarnMiles = model.EarnMiles,
            NumberOfRows = model.NumberOfRows,
            NumberOfSeats = model.NumberOfSeats,
            Price = model.Price,
            AircraftId = model.AircraftId
        });

        TempData["Success"] = $"Travel class '{model.Type}' created.";
        return RedirectToAction("TravelClasses");
    }

    [HttpGet]
    public async Task<IActionResult> EditTravelClass(int id)
    {
        var entity = await travelClasses.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await PopulateAircraftDropdown();
        return View(new TravelClassFormViewModel
        {
            Id = entity.Id,
            Type = entity.Type,
            Meal = entity.Meal,
            PriorityBoarding = entity.PriorityBoarding,
            ReservedSeat = entity.ReservedSeat,
            EarnMiles = entity.EarnMiles,
            NumberOfRows = entity.NumberOfRows,
            NumberOfSeats = entity.NumberOfSeats,
            Price = entity.Price,
            AircraftId = entity.AircraftId
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTravelClass(TravelClassFormViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await PopulateAircraftDropdown();
            return View(model);
        }

        await travelClasses.UpdateAsync(model.Id, tc =>
        {
            tc.Type = model.Type;
            tc.Meal = model.Meal;
            tc.PriorityBoarding = model.PriorityBoarding;
            tc.ReservedSeat = model.ReservedSeat;
            tc.EarnMiles = model.EarnMiles;
            tc.NumberOfRows = model.NumberOfRows;
            tc.NumberOfSeats = model.NumberOfSeats;
            tc.Price = model.Price;
            tc.AircraftId = model.AircraftId;
        });

        TempData["Success"] = $"Travel class '{model.Type}' updated.";
        return RedirectToAction("TravelClasses");
    }

    // ── Flight CRUD ─────────────────────────────────────────────

    [HttpGet]
    public IActionResult CreateFlight() => View(new FlightFormViewModel());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateFlight(FlightFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await flights.AddAsync(new Flight { Number = model.Number });

        TempData["Success"] = $"Flight '{model.Number}' created.";
        return RedirectToAction("Flights");
    }

    [HttpGet]
    public async Task<IActionResult> EditFlight(int id)
    {
        var entity = await flights.GetByIdAsync(id);
        if (entity is null) return NotFound();

        return View(new FlightFormViewModel
        {
            Id = entity.Id,
            Number = entity.Number
        });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> EditFlight(FlightFormViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await flights.UpdateAsync(model.Id, f => f.Number = model.Number);

        TempData["Success"] = $"Flight '{model.Number}' updated.";
        return RedirectToAction("Flights");
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

    private async Task PopulateManufacturersDropdown()
    {
        ViewBag.Manufacturers = new SelectList(
            await manufacturers.GetAll().Where(m => !m.IsDeleted).OrderBy(m => m.Name).ToListAsync(),
            "Id", "Name");
    }

    private async Task PopulateRoutesDropdown()
    {
        var routeList = await routes.GetAll()
            .Where(r => !r.IsDeleted)
            .Include(r => r.Origin)
            .Include(r => r.Destination)
            .ToListAsync();

        ViewBag.Routes = new SelectList(
            routeList.Select(r => new
            {
                r.Id,
                Display = $"{r.Origin?.Abbreviation ?? "?"} → {r.Destination?.Abbreviation ?? "?"} ({r.DistanceInKm} km)"
            }),
            "Id", "Display");
    }

    private async Task PopulateAircraftDropdown()
    {
        var aircraftList = await aircrafts.GetAll()
            .Where(a => !a.IsDeleted)
            .Include(a => a.AircraftManufacturer)
            .ToListAsync();

        ViewBag.Aircraft = new SelectList(
            aircraftList.Select(a => new
            {
                a.Id,
                Display = $"{a.AircraftManufacturer?.Name} {a.Model} ({a.TotalSeats} seats)"
            }),
            "Id", "Display");
    }
}
