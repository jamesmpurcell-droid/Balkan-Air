namespace BalkanAir.Api.IntegrationTests;

using BalkanAir.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BalkanAirDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<BalkanAirDbContext>(options =>
                options.UseInMemoryDatabase("IntegrationTests"));
        });

        builder.UseEnvironment("Development");
    }
}
