using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailPOS.API.DTOs.Products;
using RetailPOS.API.Services;
using RetailPOS.API.Shared;

namespace RetailPOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductsController(IProductService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] string? category, [FromQuery] bool? lowStock) =>
            Ok(new ApiResponse<List<ProductResponseDto>> { Success = true, Data = await _service.GetAllAsync(category, lowStock) });

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _service.GetByIdAsync(id);
            return Ok(new ApiResponse<ProductResponseDto> { Success = true, Data = product });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var product = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = product.Id }, new ApiResponse<ProductResponseDto> { Success = true, Data = product });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
        {
            var product = await _service.UpdateAsync(id, dto);
            return Ok(new ApiResponse<ProductResponseDto> { Success = true, Data = product });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteAsync(id);
            return success
                ? Ok(new ApiResponse<object> { Success = true, Message = "Deleted." })
                : NotFound(new ApiResponse<object> { Success = false, Message = "Not found." });
        }
    }
}
