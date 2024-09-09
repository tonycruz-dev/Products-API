using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.API.DTOs;
using Products.API.Interfaces;
using Products.API.Models;

namespace Products.API.Controllers;



/// <summary>
/// Controller for managing products.
/// </summary>

[Authorize("api")] //Secure this controller with authentication
[Route("api/[controller]")]
[ApiController]
public class ProductsController(IProductService productService) : ControllerBase
{

    /// <summary>
    /// Retrieves a product by its ID.
    /// </summary>
    /// <param name="id">The ID of the product.</param>
    /// <returns>The product with the specified ID.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var retrievedProduct = await productService.GetProductAsync(id);

        if (retrievedProduct == null)
        {
            return NotFound();
        }

        return retrievedProduct;
    }

    /// <summary>
    /// Creates a new product.
    /// </summary>
    /// <param name="productToAdd">The product to create.</param>
    /// <returns>The result of the operation.</returns>
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct([FromBody] AddProductDto productToAdd)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newProduct = new Product
        {
            Name = productToAdd.Name,
            Colour = productToAdd.Colour,
            Price = productToAdd.Price
        };
        productService.AddProduct(newProduct);
        var saveSuccess = await productService.SaveAllAsyc();
        
        if (!saveSuccess)
        {
            return BadRequest("Could not save changes to Database");
        }
        return CreatedAtAction(nameof(GetProductById), new { id = newProduct.Id }, newProduct);

    }

    /// <summary>
    /// Returns a list of all products.
    /// </summary>
    /// <returns>The list of products.</returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
    {
        var products = await productService.GetAllProducts();
        return Ok(products);
    }

    /// <summary>
    /// Returns products filtered by color.
    /// </summary>
    /// <param name="color">The color to filter by.</param>
    /// <returns>The list of products matching the color.</returns>
    [HttpGet("color/{color}")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByColor(string color)
    {
        var products = await productService.GetProductsByColour(color);

        return Ok(products);
    }
}
