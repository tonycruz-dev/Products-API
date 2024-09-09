using Microsoft.EntityFrameworkCore;
using Products.API.Models;

namespace Products.API.Data;


/// <summary>
/// Represents the data context for the application.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DataContext"/> class.
/// </remarks>
/// <param name="options">The options for configuring the data context.</param>
public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{

    /// <summary>
    /// Gets or sets the collection of products.
    /// </summary>
    public DbSet<Product> Products { get; set; } = null!;
}
