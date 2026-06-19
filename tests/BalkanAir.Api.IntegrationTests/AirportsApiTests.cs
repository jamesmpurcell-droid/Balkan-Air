namespace BalkanAir.Api.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using BalkanAir.Domain.Entities;

public class AirportsApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/airports");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreatesAirportAndReturnsCreated()
    {
        var client = factory.CreateClient();

        var airport = new Airport
        {
            Name = "Sofia Airport",
            Abbreviation = "SOF",
            CountryId = 1,
        };

        var response = await client.PostAsJsonAsync("/api/airports", airport);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    [Fact]
    public async Task Get_NotFound_Returns404()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/airports/999");
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
