using RetailPOS.API.DTOs.Auth;

namespace RetailPOS.API.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    }
}
