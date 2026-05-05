using Microsoft.AspNetCore.Mvc;
using RetailPOS.API.DTOs.Auth;
using RetailPOS.API.Services;
using RetailPOS.API.Shared;

namespace RetailPOS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Registration successful.",
                Data = result
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            return Ok(new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = result
            });
        }
    }
}