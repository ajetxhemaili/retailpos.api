using System.ComponentModel.DataAnnotations;

namespace RetailPOS.API.DTOs.Sales
{
    public class CreateSaleDto
    {
        [Required, StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, Mobile

        [Required, MinLength(1)]
        public List<CreateSaleItemDto> Items { get; set; } = new();
    }

    public class CreateSaleItemDto
    {
        [Required]
        public int ProductId { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
