namespace BalkanAir.Data;

using BalkanAir.Domain.Entities;
using BalkanAir.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider services)
    {
        var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("SeedData");

        await using var context = services.GetRequiredService<BalkanAirDbContext>();

        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }
        else
        {
            await context.Database.EnsureCreatedAsync();
        }

        if (await context.Countries.AnyAsync())
        {
            logger.LogInformation("Database already seeded — skipping");
            return;
        }

        logger.LogInformation("Seeding database...");

        // --- Roles ---
        var roleManager = services.GetService<RoleManager<IdentityRole>>();
        if (roleManager is not null)
        {
            foreach (var role in new[] { "User", "Administrator" })
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }

        // --- Countries ---
        var countries = new[]
        {
            new Country { Name = "Bulgaria", Abbreviation = "BG" },
            new Country { Name = "United Kingdom", Abbreviation = "GB" },
            new Country { Name = "Germany", Abbreviation = "DE" },
            new Country { Name = "France", Abbreviation = "FR" },
            new Country { Name = "Italy", Abbreviation = "IT" },
            new Country { Name = "Greece", Abbreviation = "GR" },
            new Country { Name = "Turkey", Abbreviation = "TR" },
            new Country { Name = "Spain", Abbreviation = "ES" },
            new Country { Name = "Serbia", Abbreviation = "RS" },
            new Country { Name = "North Macedonia", Abbreviation = "MK" },
        };
        context.Countries.AddRange(countries);
        await context.SaveChangesAsync();

        // --- Airports ---
        var airports = new[]
        {
            new Airport { Name = "Sofia Airport", Abbreviation = "SOF", CountryId = countries[0].Id },
            new Airport { Name = "London Heathrow", Abbreviation = "LHR", CountryId = countries[1].Id },
            new Airport { Name = "Frankfurt Airport", Abbreviation = "FRA", CountryId = countries[2].Id },
            new Airport { Name = "Paris Charles de Gaulle", Abbreviation = "CDG", CountryId = countries[3].Id },
            new Airport { Name = "Rome Fiumicino", Abbreviation = "FCO", CountryId = countries[4].Id },
            new Airport { Name = "Athens International", Abbreviation = "ATH", CountryId = countries[5].Id },
            new Airport { Name = "Istanbul Airport", Abbreviation = "IST", CountryId = countries[6].Id },
            new Airport { Name = "Barcelona El Prat", Abbreviation = "BCN", CountryId = countries[7].Id },
            new Airport { Name = "Belgrade Nikola Tesla", Abbreviation = "BEG", CountryId = countries[8].Id },
            new Airport { Name = "Skopje Alexander the Great", Abbreviation = "SKP", CountryId = countries[9].Id },
            new Airport { Name = "Varna Airport", Abbreviation = "VAR", CountryId = countries[0].Id },
            new Airport { Name = "Burgas Airport", Abbreviation = "BOJ", CountryId = countries[0].Id },
        };
        context.Airports.AddRange(airports);
        await context.SaveChangesAsync();

        // --- Routes (hub: Sofia) ---
        var routes = new[]
        {
            new Route { OriginId = airports[0].Id, DestinationId = airports[1].Id, DistanceInKm = 2012 }, // SOF -> LHR
            new Route { OriginId = airports[1].Id, DestinationId = airports[0].Id, DistanceInKm = 2012 }, // LHR -> SOF
            new Route { OriginId = airports[0].Id, DestinationId = airports[2].Id, DistanceInKm = 1322 }, // SOF -> FRA
            new Route { OriginId = airports[2].Id, DestinationId = airports[0].Id, DistanceInKm = 1322 }, // FRA -> SOF
            new Route { OriginId = airports[0].Id, DestinationId = airports[3].Id, DistanceInKm = 1748 }, // SOF -> CDG
            new Route { OriginId = airports[0].Id, DestinationId = airports[4].Id, DistanceInKm = 900 },  // SOF -> FCO
            new Route { OriginId = airports[0].Id, DestinationId = airports[5].Id, DistanceInKm = 524 },  // SOF -> ATH
            new Route { OriginId = airports[5].Id, DestinationId = airports[0].Id, DistanceInKm = 524 },  // ATH -> SOF
            new Route { OriginId = airports[0].Id, DestinationId = airports[6].Id, DistanceInKm = 493 },  // SOF -> IST
            new Route { OriginId = airports[6].Id, DestinationId = airports[0].Id, DistanceInKm = 493 },  // IST -> SOF
            new Route { OriginId = airports[0].Id, DestinationId = airports[8].Id, DistanceInKm = 394 },  // SOF -> BEG
            new Route { OriginId = airports[0].Id, DestinationId = airports[9].Id, DistanceInKm = 177 },  // SOF -> SKP
        };
        context.Routes.AddRange(routes);
        await context.SaveChangesAsync();

        // --- Fares ---
        var fares = new[]
        {
            new Fare { Price = 149.99m, RouteId = routes[0].Id },  // SOF -> LHR
            new Fare { Price = 159.99m, RouteId = routes[1].Id },  // LHR -> SOF
            new Fare { Price = 119.99m, RouteId = routes[2].Id },  // SOF -> FRA
            new Fare { Price = 129.99m, RouteId = routes[3].Id },  // FRA -> SOF
            new Fare { Price = 139.99m, RouteId = routes[4].Id },  // SOF -> CDG
            new Fare { Price = 109.99m, RouteId = routes[5].Id },  // SOF -> FCO
            new Fare { Price = 79.99m, RouteId = routes[6].Id },   // SOF -> ATH
            new Fare { Price = 89.99m, RouteId = routes[7].Id },   // ATH -> SOF
            new Fare { Price = 69.99m, RouteId = routes[8].Id },   // SOF -> IST
            new Fare { Price = 74.99m, RouteId = routes[9].Id },   // IST -> SOF
            new Fare { Price = 59.99m, RouteId = routes[10].Id },  // SOF -> BEG
            new Fare { Price = 49.99m, RouteId = routes[11].Id },  // SOF -> SKP
        };
        context.Fares.AddRange(fares);

        // --- Manufacturer + Aircraft ---
        var manufacturers = new[]
        {
            new AircraftManufacturer { Name = "Airbus" },
            new AircraftManufacturer { Name = "Boeing" },
            new AircraftManufacturer { Name = "Embraer" },
        };
        context.AircraftManufacturers.AddRange(manufacturers);
        await context.SaveChangesAsync();

        var aircraft = new[]
        {
            new Aircraft { Model = "A320neo", TotalSeats = 180, AircraftManufacturerId = manufacturers[0].Id },
            new Aircraft { Model = "A321neo", TotalSeats = 220, AircraftManufacturerId = manufacturers[0].Id },
            new Aircraft { Model = "737 MAX 8", TotalSeats = 189, AircraftManufacturerId = manufacturers[1].Id },
            new Aircraft { Model = "E190-E2", TotalSeats = 114, AircraftManufacturerId = manufacturers[2].Id },
        };
        context.Aircraft.AddRange(aircraft);
        await context.SaveChangesAsync();

        // --- Travel Classes ---
        var travelClasses = new[]
        {
            new TravelClass { Type = TravelClassType.Economy, Meal = "Snack box", PriorityBoarding = false, ReservedSeat = true, EarnMiles = true, NumberOfRows = 25, NumberOfSeats = 150, Price = 0m, AircraftId = aircraft[0].Id },
            new TravelClass { Type = TravelClassType.Business, Meal = "Full service", PriorityBoarding = true, ReservedSeat = true, EarnMiles = true, NumberOfRows = 5, NumberOfSeats = 30, Price = 120m, AircraftId = aircraft[0].Id },
            new TravelClass { Type = TravelClassType.Economy, Meal = "Snack box", PriorityBoarding = false, ReservedSeat = true, EarnMiles = true, NumberOfRows = 30, NumberOfSeats = 186, Price = 0m, AircraftId = aircraft[1].Id },
            new TravelClass { Type = TravelClassType.Business, Meal = "Full service", PriorityBoarding = true, ReservedSeat = true, EarnMiles = true, NumberOfRows = 6, NumberOfSeats = 34, Price = 150m, AircraftId = aircraft[1].Id },
        };
        context.TravelClasses.AddRange(travelClasses);

        // --- Flight Statuses ---
        var statuses = new[]
        {
            new FlightStatus { Name = "Scheduled" },
            new FlightStatus { Name = "Boarding" },
            new FlightStatus { Name = "Departed" },
            new FlightStatus { Name = "Arrived" },
            new FlightStatus { Name = "Cancelled" },
            new FlightStatus { Name = "Delayed" },
        };
        context.FlightStatuses.AddRange(statuses);
        await context.SaveChangesAsync();

        // --- Flights ---
        var flights = new[]
        {
            new Flight { Number = "BK101" },
            new Flight { Number = "BK102" },
            new Flight { Number = "BK201" },
            new Flight { Number = "BK202" },
            new Flight { Number = "BK301" },
            new Flight { Number = "BK401" },
            new Flight { Number = "BK501" },
            new Flight { Number = "BK502" },
            new Flight { Number = "BK601" },
            new Flight { Number = "BK602" },
            new Flight { Number = "BK701" },
            new Flight { Number = "BK801" },
        };
        context.Flights.AddRange(flights);
        await context.SaveChangesAsync();

        // --- Flight Legs (one per route, mapped to flights) ---
        var now = DateTime.UtcNow.Date.AddDays(1).AddHours(6);
        var legs = new[]
        {
            new FlightLeg { FlightId = flights[0].Id, RouteId = routes[0].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[1].Id, ScheduledDepartureDateTime = now, ScheduledArrivalDateTime = now.AddHours(3).AddMinutes(20) },
            new FlightLeg { FlightId = flights[1].Id, RouteId = routes[1].Id, DepartureAirportId = airports[1].Id, ArrivalAirportId = airports[0].Id, ScheduledDepartureDateTime = now.AddHours(5), ScheduledArrivalDateTime = now.AddHours(8).AddMinutes(20) },
            new FlightLeg { FlightId = flights[2].Id, RouteId = routes[2].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[2].Id, ScheduledDepartureDateTime = now.AddHours(1), ScheduledArrivalDateTime = now.AddHours(3) },
            new FlightLeg { FlightId = flights[3].Id, RouteId = routes[3].Id, DepartureAirportId = airports[2].Id, ArrivalAirportId = airports[0].Id, ScheduledDepartureDateTime = now.AddHours(6), ScheduledArrivalDateTime = now.AddHours(8) },
            new FlightLeg { FlightId = flights[4].Id, RouteId = routes[4].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[3].Id, ScheduledDepartureDateTime = now.AddHours(2), ScheduledArrivalDateTime = now.AddHours(4).AddMinutes(30) },
            new FlightLeg { FlightId = flights[5].Id, RouteId = routes[5].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[4].Id, ScheduledDepartureDateTime = now.AddHours(3), ScheduledArrivalDateTime = now.AddHours(4).AddMinutes(40) },
            new FlightLeg { FlightId = flights[6].Id, RouteId = routes[6].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[5].Id, ScheduledDepartureDateTime = now.AddHours(4), ScheduledArrivalDateTime = now.AddHours(5).AddMinutes(10) },
            new FlightLeg { FlightId = flights[7].Id, RouteId = routes[7].Id, DepartureAirportId = airports[5].Id, ArrivalAirportId = airports[0].Id, ScheduledDepartureDateTime = now.AddHours(7), ScheduledArrivalDateTime = now.AddHours(8).AddMinutes(10) },
            new FlightLeg { FlightId = flights[8].Id, RouteId = routes[8].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[6].Id, ScheduledDepartureDateTime = now.AddHours(2), ScheduledArrivalDateTime = now.AddHours(3).AddMinutes(15) },
            new FlightLeg { FlightId = flights[9].Id, RouteId = routes[9].Id, DepartureAirportId = airports[6].Id, ArrivalAirportId = airports[0].Id, ScheduledDepartureDateTime = now.AddHours(5), ScheduledArrivalDateTime = now.AddHours(6).AddMinutes(15) },
            new FlightLeg { FlightId = flights[10].Id, RouteId = routes[10].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[8].Id, ScheduledDepartureDateTime = now.AddHours(8), ScheduledArrivalDateTime = now.AddHours(9) },
            new FlightLeg { FlightId = flights[11].Id, RouteId = routes[11].Id, DepartureAirportId = airports[0].Id, ArrivalAirportId = airports[9].Id, ScheduledDepartureDateTime = now.AddHours(9), ScheduledArrivalDateTime = now.AddHours(9).AddMinutes(40) },
        };
        context.FlightLegs.AddRange(legs);
        await context.SaveChangesAsync();

        // --- Leg Instances (upcoming week, one per leg per day for 7 days) ---
        var scheduledStatusId = statuses[0].Id;
        var instances = new List<LegInstance>();
        for (int day = 0; day < 7; day++)
        {
            for (int i = 0; i < legs.Length; i++)
            {
                var leg = legs[i];
                var offset = TimeSpan.FromDays(day);
                instances.Add(new LegInstance
                {
                    FlightLegId = leg.Id,
                    DepartureDateTime = leg.ScheduledDepartureDateTime + offset,
                    ArrivalDateTime = leg.ScheduledArrivalDateTime + offset,
                    Price = fares[i].Price,
                    FlightStatusId = scheduledStatusId,
                    AircraftId = aircraft[i % aircraft.Length].Id,
                });
            }
        }
        context.LegInstances.AddRange(instances);

        // --- Categories + News ---
        var categories = new[]
        {
            new Category { Name = "Routes" },
            new Category { Name = "Fleet" },
            new Category { Name = "Company" },
        };
        context.Categories.AddRange(categories);
        await context.SaveChangesAsync();

        context.News.AddRange(
            new News { Title = "New Route: Sofia to Barcelona!", Content = "We are excited to announce our newest route connecting Sofia to Barcelona starting next month. Flights will operate three times weekly with our Airbus A320neo fleet.", DateCreated = DateTime.UtcNow.AddDays(-5), CategoryId = categories[0].Id },
            new News { Title = "Fleet Expansion: Two New A321neo Aircraft", Content = "Balkan Air welcomes two brand-new Airbus A321neo aircraft to our fleet, increasing capacity on our most popular routes to London and Frankfurt.", DateCreated = DateTime.UtcNow.AddDays(-3), CategoryId = categories[1].Id },
            new News { Title = "Summer 2026 Schedule Released", Content = "Our summer schedule is now live with over 80 weekly departures across 12 destinations. Book early for the best fares to Athens, Istanbul, and Rome.", DateCreated = DateTime.UtcNow.AddDays(-1), CategoryId = categories[0].Id },
            new News { Title = "Balkan Air Celebrates 10 Years", Content = "This year marks our 10th anniversary of connecting the Balkans with Europe. Thank you to our loyal passengers — we have carried over 5 million travellers since our founding.", DateCreated = DateTime.UtcNow, CategoryId = categories[2].Id }
        );

        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded with {Countries} countries, {Airports} airports, {Routes} routes, {Flights} flights, {Instances} leg instances",
            countries.Length, airports.Length, routes.Length, flights.Length, instances.Count);
    }
}
