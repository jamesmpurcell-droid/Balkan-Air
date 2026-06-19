namespace BalkanAir.Data;

using BalkanAir.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class BalkanAirDbContext : DbContext
{
    public BalkanAirDbContext(DbContextOptions<BalkanAirDbContext> options)
        : base(options)
    {
    }

    public DbSet<Aircraft> Aircraft => Set<Aircraft>();
    public DbSet<AircraftManufacturer> AircraftManufacturers => Set<AircraftManufacturer>();
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Baggage> Baggages => Set<Baggage>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<CreditCard> CreditCards => Set<CreditCard>();
    public DbSet<Fare> Fares => Set<Fare>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightLeg> FlightLegs => Set<FlightLeg>();
    public DbSet<FlightStatus> FlightStatuses => Set<FlightStatus>();
    public DbSet<LegInstance> LegInstances => Set<LegInstance>();
    public DbSet<News> News => Set<News>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Route> Routes => Set<Route>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<TravelClass> TravelClasses => Set<TravelClass>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BalkanAirDbContext).Assembly);
    }
}
