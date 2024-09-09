using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Products.API.Data;
using WebMotions.Fake.Authentication.JwtBearer;

namespace Products.API.IntegrationTest.Helpers;



/// <summary>
/// Factory class for creating the web application for integration testing.
/// </summary>
public class ProductApiApplicationFactory : WebApplicationFactory<Program>
{
    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<DataContext>));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(GetConnectionString());
            });
            var dbContext = CreateContext(services);
            dbContext.Database.EnsureDeleted();
            dbContext.Database.Migrate();

            services.RemoveAll<IAuthorizationHandler>();

            // Add a fake policy that always allows access
            services.AddAuthorization(options =>
            {
                options.AddPolicy("api", policy =>
                {
                    policy.RequireAssertion(_ => true); // Always return true, bypassing authorization
                });
            });

            DBHelper.InitProductsDBForTests(dbContext);
        });

    }

    /// <summary>
    /// Gets the connection string for the SQLite database.
    /// </summary>
    /// <returns>The connection string.</returns>
    private static string? GetConnectionString()
    {
        return "Data Source=productsTest.db";
    }

    /// <summary>
    /// Creates an instance of the DataContext using the provided services.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The DataContext instance.</returns>
    private static DataContext CreateContext(IServiceCollection services)
    {
        var sp = services.BuildServiceProvider();
        var scope = sp.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        return context;
    }
}
