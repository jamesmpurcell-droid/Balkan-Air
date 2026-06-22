namespace BalkanAir.Web.Tests;

using System.Net;

[Collection("WebApp")]
public class PublicPagesTests(WebAppFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task FlightsPage_ReturnsOk()
    {
        var response = await _client.GetAsync("/Flights");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task NewsPage_ReturnsOk()
    {
        var response = await _client.GetAsync("/News");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task BookingSearch_ReturnsOk()
    {
        var response = await _client.GetAsync("/Booking/Search");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Search Flights", content);
    }

    [Fact]
    public async Task FlightsPage_ContainsTitle()
    {
        var content = await (await _client.GetAsync("/Flights")).Content.ReadAsStringAsync();
        Assert.Contains("Flights", content);
    }
}
