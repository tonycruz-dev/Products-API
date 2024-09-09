namespace Products.API.DTOs;


/// <summary>
/// Represents the data transfer object for adding a product.
/// </summary>
public class AddProductDto
{
    /// <summary>
    /// Gets or sets the name of the product.
    /// </summary>
    /// <value>The name of the product.</value>
    public required string Name { get; set; }

    /// <summary>
    /// Gets or sets the color of the product.
    /// </summary>
    /// <value>The color of the product.</value>
    public required string Colour { get; set; }

    /// <summary>
    /// Gets or sets the price of the product.
    /// </summary>
    /// <value>The price of the product.</value>
    public required decimal Price { get; set; }
}
