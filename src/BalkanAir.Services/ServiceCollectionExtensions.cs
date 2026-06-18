namespace BalkanAir.Services;

using BalkanAir.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBalkanAirServices(this IServiceCollection services)
    {
        services.AddScoped<IAircraftsService, AircraftsService>();
        services.AddScoped<IAircraftManufacturersService, AircraftManufacturersService>();
        services.AddScoped<IAirportsService, AirportsService>();
        services.AddScoped<IBaggageService, BaggageService>();
        services.AddScoped<IBookingsService, BookingsService>();
        services.AddScoped<ICategoriesService, CategoriesService>();
        services.AddScoped<ICommentsService, CommentsService>();
        services.AddScoped<ICountriesService, CountriesService>();
        services.AddScoped<ICreditCardsService, CreditCardsService>();
        services.AddScoped<IFaresService, FaresService>();
        services.AddScoped<IFlightsService, FlightsService>();
        services.AddScoped<IFlightLegsService, FlightLegsService>();
        services.AddScoped<IFlightStatusesService, FlightStatusesService>();
        services.AddScoped<ILegInstancesService, LegInstancesService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<IRoutesService, RoutesService>();
        services.AddScoped<ISeatsService, SeatsService>();
        services.AddScoped<ITravelClassesService, TravelClassesService>();
        services.AddScoped<IUserNotificationsService, UserNotificationsService>();
        services.AddScoped<IUsersService, UsersService>();

        return services;
    }
}
