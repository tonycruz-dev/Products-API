using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Products.API.Data;
using Products.API.DTOs;
using Products.API.IntegrationTest.Helpers;
using Products.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Products.API.IntegrationTest.Controllers;

public class ProductsControllerTests : IClassFixture<ProductApiApplicationFactory>, IAsyncLifetime
{

    private readonly ProductApiApplicationFactory _factory;
    private HttpClient _client;
    public ProductsControllerTests(ProductApiApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
       
    }

    [Fact]
    public async Task GetProductById_ReturnsProduct_WhenExists()
    {

        //act
        var requestProducts = await _client.GetAsync($"api/Products");


        var products = await requestProducts.Content.ReadFromJsonAsync<List<Product>>();
        // Arrange

        // Act
        var response = await _client.GetAsync($"api/Products/{products![0].Id}");


        // Assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("Product 1", responseString?.Name);
    }
    /// <summary>
    /// get product by id returns not found when product does not exist
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        //act
        var response = await _client.GetAsync("/api/Products/999");

        //assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    /// <summary>
    /// post product with valid product returns created at action
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostProduct_ValidProduct_ReturnsCreatedAtAction()
    {
        //act

        var productToAdd = new AddProductDto { Name = "New Product", Colour = "Blue", Price = 150 };

        var response = await _client.PostAsJsonAsync("/api/Products", productToAdd);

        //assert
        response.EnsureSuccessStatusCode();
        var responseString = await response.Content.ReadFromJsonAsync<Product>();
        Assert.Equal("New Product", responseString?.Name);
    }
    /// <summary>
    /// post customer with invalid customer returns bad request
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task PostCustomer_InvalidCustomer_ReturnsBadRequest()
    {
        //arrange
        var addProductDto = new AddProductDto { Colour = "Red", Price = 100, Name = null };

        //act
        var response = await _client.PostAsJsonAsync("/api/Products", addProductDto);

        //assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
    /// <summary>
    /// Get all products returns ok with product list
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task GetAllProducts_ReturnsOk_WithProductList()
    {
        //act
        var response = await _client.GetAsync($"api/Products");

        //assert
        response.EnsureSuccessStatusCode();

        var SupplierResponse = await response.Content.ReadFromJsonAsync<List<Product>>();
        Assert.True(SupplierResponse?.Count() > 0);
    }
    public Task InitializeAsync() => Task.CompletedTask;
    public Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DataContext>();
        DBHelper.ReinitializeDbForTests(context);
        return Task.CompletedTask;
    }

}
