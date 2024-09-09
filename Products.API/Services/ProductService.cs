using Microsoft.EntityFrameworkCore;
using Products.API.Data;
using Products.API.Interfaces;
using Products.API.Models;

namespace Products.API.Services;

/// <summary>
/// Service class for managing products.
/// </summary>
public class ProductService : IProductService
{
    private readonly DataContext dataContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductService"/> class.
    /// </summary>
    /// <param name="dataContext">The data context.</param>
    public ProductService(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }

    /// <summary>
    /// Gets a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product with the specified ID, or null if not found.</returns>
    public async Task<Product?> GetProductAsync(int id)
    {
        return await dataContext.Products.FirstOrDefaultAsync(p => p.Id == id);
    }

    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="product">The product to add.</param>
    public void AddProduct(Product product)
    {
        dataContext.Products.Add(product);
    }

    /// <summary>
    /// Gets all products.
    /// </summary>
    /// <returns>A collection of all products.</returns>
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await dataContext.Products.ToListAsync();
    }

    /// <summary>
    /// Saves all changes made to the data context.
    /// </summary>
    /// <returns>True if any changes were saved, false otherwise.</returns>
    public async Task<bool> SaveAllAsyc()
    {
        return await dataContext.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Gets products by color.
    /// </summary>
    /// <param name="color">The color of the products.</param>
    /// <returns>A collection of products with the specified color.</returns>
    public async Task<IEnumerable<Product>> GetProductsByColour(string color)
    {
        return await dataContext.Products.Where(p => p.Colour.ToLower() == color.ToLower()).ToListAsync();
    }
}
