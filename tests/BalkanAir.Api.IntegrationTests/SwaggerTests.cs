namespace BalkanAir.Api.IntegrationTests;

using System.Net;

public class SwaggerTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task SwaggerJson_ReturnsOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/swagger/v1/swagger.json");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Balkan Air API", content);
    }

    [Fact]
    public async Task SwaggerUI_ReturnsOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/swagger/index.html");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
