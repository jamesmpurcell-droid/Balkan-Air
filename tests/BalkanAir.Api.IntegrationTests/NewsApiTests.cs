namespace BalkanAir.Api.IntegrationTests;

using System.Net;
using System.Net.Http.Json;
using BalkanAir.Domain.Entities;

public class NewsApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetAll_ReturnsOk()
    {
        var client = factory.CreateClient();
        var response = await client.GetAsync("/api/news");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Post_CreatesNewsAndReturnsCreated()
    {
        var client = factory.CreateClient();

        var item = new News
        {
            Title = "Test News",
            Content = "Integration test content.",
            DateCreated = DateTime.UtcNow,
            CategoryId = 1,
        };

        var response = await client.PostAsJsonAsync("/api/news", item);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
