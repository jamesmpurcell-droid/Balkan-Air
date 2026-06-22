namespace BalkanAir.Web.Tests;

using BalkanAir.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public class WebAppFactory : WebApplicationFactory<Program>
{
    private readonly string _dbName = "WebTests_" + Guid.NewGuid();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<BalkanAirDbContext>));

            if (descriptor is not null)
                services.Remove(descriptor);

            services.AddDbContext<BalkanAirDbContext>(options =>
                options.UseInMemoryDatabase(_dbName));
        });

        builder.UseEnvironment("Development");
    }
}

[CollectionDefinition("WebApp")]
public class WebAppCollection : ICollectionFixture<WebAppFactory>;
