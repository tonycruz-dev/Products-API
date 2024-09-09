using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Products.API.Controllers;
using Products.API.DTOs;
using Products.API.Interfaces;
using Products.API.Models;

namespace Products.API.UnitTest.Controllers;

/// <summary>
/// Products controller tests
/// </summary>
public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockRepo;
    private readonly Fixture _fixture;
    private readonly ProductsController _controller;
    public ProductsControllerTests()
    {
        _mockRepo = new Mock<IProductService>();
        _fixture = new Fixture();
        _controller = new ProductsController(_mockRepo.Object);
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }
    /// <summary>
    /// get product with valid id returns product.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProduct_WithValidId_ReturnsProduct()
    {
        // Arrange
        var product = _fixture.Create<Product>();
        _mockRepo.Setup(x => x.GetProductAsync(product.Id)).ReturnsAsync(product);

        // Act
        var result = await _controller.GetProductById(product.Id);

        // Assert
        Assert.Equal(product.Name, result.Value?.Name);
    }
    /// <summary>
    /// get product by id returns not found when product does not exist
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        // Arrange
        int id = 1;
        _mockRepo.Setup(repo => repo.GetProductAsync(id))
            .ReturnsAsync((Product)null);

        // Act
        var result = await _controller.GetProductById(id);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    /// <summary>
    /// post product with valid product returns created at action.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostProduct_WithValidProduct_ReturnsCreatedAtAction()
    {
        // Arrange
        var product = _fixture.Create<AddProductDto>();
        _mockRepo.Setup(x => x.AddProduct(It.IsAny<Product>()));
        _mockRepo.Setup(x => x.SaveAllAsyc()).ReturnsAsync(true);

        // Act
        var result = await _controller.PostProduct(product);

        // Assert
        Assert.IsType<CreatedAtActionResult>(result.Result);
    }
    /// <summary>
    /// post customer with invalid customer returns bad request
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostCustomer_InvalidCustomer_ReturnsBadRequest()
    {
        // Arrange
        var addProductDto = new AddProductDto { Colour = "Red", Price = 100, Name = null };
        _mockRepo.Setup(r => r.SaveAllAsyc()).ReturnsAsync(false);

        // Act
        var result = await _controller.PostProduct(addProductDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Could not save changes to Database", badRequestResult.Value);
    }
    /// <summary>
    /// post customer with invalid customer returns model state is valid
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostCustomer_InvalidCustomer_ReturnsModelStateIsValid()
    {
        // Arrange
        // Arrange
        var productToAdd = new AddProductDto { Name = "Invalid Product", Colour = "", Price = -50 };

        // Simulate invalid model state
        _controller.ModelState.AddModelError("Colour", "Required");

        // Act
        var result = await _controller.PostProduct(productToAdd);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }
    /// <summary>
    /// Get all products returns ok with product list
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllProducts_ReturnsOk_WithProductList()
    {
        // Arrange
        var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Colour = "Red", Price = 100 },
                new() { Id = 2, Name = "Product 2", Colour = "Blue", Price = 200 },
                new() { Id = 3, Name = "Product 3", Colour = "Green", Price = 300 }
            };
        var products = _fixture.CreateMany<Product>(3);
        // Mock the GetAllProducts method to return the mock products
        _mockRepo.Setup(service => service.GetAllProducts()).ReturnsAsync(products);

        // Act
        var result = await _controller.GetAllProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifies that the response is OkObjectResult
        var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value); // Verifies that the value inside OkObjectResult is a list of Product
        Assert.Equal(3, returnProducts.Count()); // Verifies that the returned list contains 3 products
    }
    /// <summary>
    /// Get all products with empty list returns ok with empty list
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllProducts_ReturnsOk_WithEmptyList()
    {
        // Arrange
        var mockProducts = new List<Product>(); // Empty product list

        // Mock the GetAllProducts method to return an empty list
        _mockRepo.Setup(service => service.GetAllProducts()).ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetAllProducts();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifies that the response is OkObjectResult
        var returnProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value); // Verifies that the value inside OkObjectResult is a list of Product
        Assert.Empty(returnProducts); // Verifies that the returned list is empty
    }
    /// <summary>
    /// Get products by color with matching products returns ok with matching products
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProductsByColor_ReturnsOk_WithMatchingProducts()
    {
        // Arrange
        string color = "Red";
        var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Product 1", Colour = "Red", Price = 100 },
                new() { Id = 2, Name = "Product 2", Colour = "Red", Price = 200 }
            };

        // Mock the GetProductsByColour method to return the mock products
        _mockRepo.Setup(service => service.GetProductsByColour(color))
                           .ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetProductsByColor(color);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifies that the response is OkObjectResult
        var returnProducts = Assert.IsType<List<Product>>(okResult.Value); // Verifies that the value inside OkObjectResult is a list of Product
        Assert.Equal(2, returnProducts.Count); // Verifies that the returned list contains 2 products
    }
    /// <summary>
    /// Get products by color with no matching products returns ok with no matching products
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProductsByColor_ReturnsOk_WithNoMatchingProducts()
    {
        // Arrange
        string color = "Blue";
        var mockProducts = new List<Product>(); // Empty product list

        // Mock the GetProductsByColour method to return an empty list
        _mockRepo.Setup(service => service.GetProductsByColour(color))
                           .ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetProductsByColor(color);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifies that the response is OkObjectResult
        var returnProducts = Assert.IsType<List<Product>>(okResult.Value); // Verifies that the value inside OkObjectResult is a list of Product
        Assert.Empty(returnProducts); // Verifies that the returned list is empty
    }
    /// <summary>
    /// Get products by color with empty color returns ok with no matching products
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProductsByColor_EmptyColor_ReturnsOk_WithNoMatchingProducts()
    {
        // Arrange
        string color = ""; // Simulate empty string for color
        var mockProducts = new List<Product>(); // Empty product list

        // Mock the GetProductsByColour method to return an empty list
        _mockRepo.Setup(service => service.GetProductsByColour(color))
                           .ReturnsAsync(mockProducts);

        // Act
        var result = await _controller.GetProductsByColor(color);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); // Verifies that the response is OkObjectResult
        var returnProducts = Assert.IsType<List<Product>>(okResult.Value); // Verifies that the value inside OkObjectResult is a list of Product
        Assert.Empty(returnProducts); // Verifies that the returned list is empty
    }

}
