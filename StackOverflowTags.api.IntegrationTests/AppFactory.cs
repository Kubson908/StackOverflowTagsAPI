using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StackOverflowTags.api.Data;
using Testcontainers.PostgreSql;

namespace StackOverflowTags.api.IntegrationTests;

public class AppFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest").WithDatabase("sotags").WithUsername("sotagsuser")
        .WithPassword("postgres").Build();

    public HttpClient HttpClient = default!;

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(_dbContainer.GetConnectionString()));
        });
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}
