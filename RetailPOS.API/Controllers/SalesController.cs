using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RetailPOS.API.DTOs.Sales;
using RetailPOS.API.Services;
using RetailPOS.API.Shared;
using System.Security.Claims;

namespace RetailPOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _service;

        public SalesController(ISalesService service) => _service = service;

        private int GetCurrentUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSaleDto dto)
        {
            var sale = await _service.CreateSaleAsync(GetCurrentUserId(), dto);
            return CreatedAtAction(nameof(Get), new { id = sale.Id }, new ApiResponse<SaleResponseDto> { Success = true, Data = sale });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var sale = await _service.GetSaleByIdAsync(id);
            return Ok(new ApiResponse<SaleResponseDto> { Success = true, Data = sale });
        }
    }
}