using Microsoft.EntityFrameworkCore;
using StockApi.Infrastructure.Context;
using StockApi.Infrastructure.Repositories;
using StockApi.Tests.Fixtures;

namespace StockApi.Tests;

[TestFixture]
public class ProductRepositoryTests
{

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
        // Configure Context Fixture for this test
        using var fixture = ConfigureFixture("GetProductsWithPagination_ReturnsCorrectPaginatedResults_Db");

        // Create repository with the real context
        var repository = new ProductRepository(fixture.Context);

        // Act
        var response = await repository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, null);

        // Asset
        Assert.That(pageNumber, Is.EqualTo(response.PageNumber));
        Assert.That(pageSize, Is.EqualTo(response.PageSize));
        Assert.That(response.Data, Has.Count.EqualTo(5));
        Assert.That(response.HasNextPage, Is.False);
        Assert.That(response.HasPreviousPage, Is.False);   
    }
}
