namespace BalkanAir.Api.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using BalkanAir.Data;
using BalkanAir.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

public class FlightsApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/flights");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Get_NotFound_Returns404()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/flights/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreatesFlightAndReturnsCreated()
    {
        var client = factory.CreateClient();

        var flight = new LegInstance
        {
            DepartureDateTime = DateTime.UtcNow.AddDays(1),
            ArrivalDateTime = DateTime.UtcNow.AddDays(1).AddHours(2),
            FlightLegId = 1,
            AircraftId = 1,
        };

        var response = await client.PostAsJsonAsync("/api/flights", flight);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
