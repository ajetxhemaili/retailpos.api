using System.ComponentModel.DataAnnotations;

namespace RetailPOS.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required, StringLength(100, MinimumLength = 3)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Cashier"; // Admin, Manager, Cashier
    }
}
