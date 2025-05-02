using Microsoft.EntityFrameworkCore;
using StockApi.Infrastructure.Context;
using StockApi.Infrastructure.Repositories;
using StockApi.Tests.Fixtures;

namespace StockApi.Tests;

[TestFixture]
public class ProductRepositoryTests
{
    private ProductRepository _repository;

    public ProductRepositoryTests()
    {
        // Configure Context Fixture for this test
        var fixture = ConfigureFixture("GetProductsWithPagination_Database");
        // Create repository with the real context
        _repository = new ProductRepository(fixture.Context);
    }

    public ProductRepositoryTestFixture ConfigureFixture(string dataBaseName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(dataBaseName)
            .Options;

        return new ProductRepositoryTestFixture(options);
    }

    [Test]
    public async Task GetProductsWithPagination_ReturnsCorrectPaginatedResults()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;

        // Act
        var response = await _repository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, null);

        // Asset
        Assert.That(pageNumber, Is.EqualTo(response.PageNumber));
        Assert.That(pageSize, Is.EqualTo(response.PageSize));
        Assert.That(response.Data, Has.Count.EqualTo(5));
        Assert.That(response.HasNextPage, Is.False);
        Assert.That(response.HasPreviousPage, Is.False);   
    }

    [Test]
    public async Task GetProductsWithPagination_FilterByAutomotiveCategory_ReturnsEmptyPaginatedResults()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        Dictionary<string, string> filters = new Dictionary<string, string>() {{ "Category", "Automotive" }};

        // Act
        var response = await _repository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, filters);

        // Asset
        Assert.That(pageNumber, Is.EqualTo(response.PageNumber));
        Assert.That(pageSize, Is.EqualTo(response.PageSize));
        Assert.That(response.Data, Has.Count.EqualTo(0));
        Assert.That(response.HasNextPage, Is.False);
        Assert.That(response.HasPreviousPage, Is.False);
    }

    [Test]
    public async Task GetProductsWithPagination_FilterByProductName_ReturnsCorrectPaginatedResults()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        Dictionary<string, string> filters = new Dictionary<string, string>() {{ "Name", "Product 3" }};

        // Act
        var response = await _repository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, filters);

        // Asset
        Assert.That(pageNumber, Is.EqualTo(response.PageNumber));
        Assert.That(pageSize, Is.EqualTo(response.PageSize));
        Assert.That(response.Data, Has.Count.EqualTo(1));
        Assert.That(response.HasNextPage, Is.False);
        Assert.That(response.HasPreviousPage, Is.False);
    }

    [Test]
    public async Task GetProductsWithPagination_FilterByProductNameAndPrice_ReturnsCorrectPaginatedResults()
    {
        // Arrange
        int pageNumber = 1;
        int pageSize = 5;
        Dictionary<string, string> filters = new Dictionary<string, string>() 
        { { "Name", "Product 5" }, { "Price", "5.99" } };

        // Act
        var response = await _repository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, filters);

        // Asset
        Assert.That(pageNumber, Is.EqualTo(response.PageNumber));
        Assert.That(pageSize, Is.EqualTo(response.PageSize));
        Assert.That(response.Data, Has.Count.EqualTo(1));
        Assert.That(response.HasNextPage, Is.False);
        Assert.That(response.HasPreviousPage, Is.False);
    }
}
