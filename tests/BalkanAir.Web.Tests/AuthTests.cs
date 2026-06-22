namespace BalkanAir.Web.Tests;

using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

[Collection("WebApp")]
public class AuthTests(WebAppFactory factory)
{
    private readonly HttpClient _client = factory.CreateClient(
        new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

    [Fact]
    public async Task LoginPage_ReturnsOk()
    {
        var response = await _client.GetAsync("/Account/Login");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Login", content);
    }

    [Fact]
    public async Task RegisterPage_ReturnsOk()
    {
        var response = await _client.GetAsync("/Account/Register");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("Register", content);
    }

    [Fact]
    public async Task Profile_RedirectsToLogin_WhenAnonymous()
    {
        var response = await _client.GetAsync("/Profile");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location?.ToString() ?? "");
    }

    [Fact]
    public async Task Administration_RedirectsToLogin_WhenAnonymous()
    {
        var response = await _client.GetAsync("/Administration");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location?.ToString() ?? "");
    }

    [Fact]
    public async Task MyBookings_RedirectsToLogin_WhenAnonymous()
    {
        var response = await _client.GetAsync("/Booking/MyBookings");

        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.Contains("/Account/Login", response.Headers.Location?.ToString() ?? "");
    }

    [Fact]
    public async Task BookingSearch_IsAccessible_WhenAnonymous()
    {
        var response = await _client.GetAsync("/Booking/Search");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}
