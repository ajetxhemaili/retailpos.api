namespace RetailPOS.API.DTOs.Auth
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
    }
}
