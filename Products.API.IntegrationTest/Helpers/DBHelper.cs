using Products.API.Data;
using Products.API.Models;

namespace Products.API.IntegrationTest.Helpers;

/// <summary>
/// Helper class for managing the test database.
/// </summary>
public static class DBHelper
{
    /// <summary>
    /// Initializes the test database with sample products.
    /// </summary>
    /// <param name="context">The data context.</param>
    public static void InitProductsDBForTests(DataContext context)
    {
        context.Products.AddRange(GetProducts());
        context.SaveChanges();
    }

    /// <summary>
    /// Reinitializes the test database by removing all products and adding sample products.
    /// </summary>
    /// <param name="context">The data context.</param>
    public static void ReinitializeDbForTests(DataContext context)
    {
        context.Products.RemoveRange(context.Products);
        context.SaveChanges();
        InitProductsDBForTests(context);
    }

    /// <summary>
    /// Gets a list of sample products.
    /// </summary>
    /// <returns>A list of sample products.</returns>
    public static List<Product> GetProducts()
    {
        return new List<Product>
            {
                new Product
                {
                    Name = "Product 1",
                    Colour = "Red",
                    Price = 10.00m
                },
                new Product
                {
                    Name = "Product 2",
                    Colour = "Blue",
                    Price = 20.00m
                },
                new Product
                {
                    Name = "Product 3",
                    Colour = "Green",
                    Price = 30.00m
                },
                new Product
                {
                    Name = "Product 4",
                    Colour = "Red",
                    Price = 40.00m
                },
                new Product
                {
                    Name = "Product 5",
                    Colour = "Blue",
                    Price = 50.00m
                },
            };
    }
}
