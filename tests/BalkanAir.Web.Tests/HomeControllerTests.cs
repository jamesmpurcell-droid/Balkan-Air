namespace BalkanAir.Web.Tests;

using System.Net;

[Collection("WebApp")]
public class HomeControllerTests(WebAppFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task HomePage_ReturnsOk_AndContainsTitle()
    {
        var response = await _client.GetAsync("/");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Welcome to Balkan Air", content);
    }

    [Fact]
    public async Task HomePage_ContainsNavbarLinks()
    {
        var content = await (await _client.GetAsync("/")).Content.ReadAsStringAsync();

        Assert.Contains("Flights", content);
        Assert.Contains("News", content);
        Assert.Contains("Login", content);
        Assert.Contains("Register", content);
    }

    [Fact]
    public async Task HomePage_ContainsBootstrap5()
    {
        var content = await (await _client.GetAsync("/")).Content.ReadAsStringAsync();

        Assert.Contains("bootstrap", content, StringComparison.OrdinalIgnoreCase);
    }
}
