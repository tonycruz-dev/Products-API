using Products.API.Models;

namespace Products.API.Interfaces;

/// <summary>
/// Represents a service for managing products.
/// </summary>
public interface IProductService
{
    /// <summary>
    /// Adds a new product.
    /// </summary>
    /// <param name="product">The product to add.</param>
    void AddProduct(Product product);

    /// <summary>
    /// Retrieves all products.
    /// </summary>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetAllProducts();

    /// <summary>
    /// Retrieves products by color.
    /// </summary>
    /// <param name="color">The color of the products to retrieve.</param>
    /// <returns>A collection of products.</returns>
    Task<IEnumerable<Product>> GetProductsByColour(string color);

    /// <summary>
    /// Retrieves a product by ID.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <returns>The product with the specified ID, or null if not found.</returns>
    Task<Product?> GetProductAsync(int id);

    /// <summary>
    /// Saves all changes asynchronously.
    /// </summary>
    /// <returns>True if the changes were successfully saved, false otherwise.</returns>
    Task<bool> SaveAllAsyc();
}
