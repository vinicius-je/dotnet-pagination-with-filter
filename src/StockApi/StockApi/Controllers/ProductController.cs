using Microsoft.AspNetCore.Mvc;
using StockApi.Domain.Dtos;
using StockApi.Domain.Interfaces;
using StockApi.Response;

namespace StockApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithPagination(
            [FromQuery] Dictionary<string, string>? filters,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 25)
        {
            if (pageSize > 250)
            {
                return BadRequest(new ApiResponse<ProductDto>(400, "Error: the page size limit is 500 records"));
            }

            if (pageSize <= 0)
            {
                return BadRequest(new ApiResponse<ProductDto>(400, "Error: the page size can not be equals or below zero"));
            }

            // Remove the pageNumber and pageSize from filter
            if (filters is not null && filters.Count >= 2)
            {
                filters.Remove("pageNumber");
                filters.Remove("pageSize");
            }

            var paginationResponse = await _productRepository.GetProductsWithPaginationAndFilter(pageNumber, pageSize, filters);

            if (paginationResponse == null)
            {
                return BadRequest(new ApiResponse<ProductDto>(400, "Error: An error occurred during the request"));
            }

            var apiResponse = new ApiResponse<PaginationResponse<ProductDto>>(200, "Success", paginationResponse);
            return Ok(apiResponse);
        }
    }
}
