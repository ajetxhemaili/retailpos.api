using System.ComponentModel.DataAnnotations;

namespace RetailPOS.API.DTOs.Products
{
    public class CreateProductDto
    {
        [Required, StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Barcode { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CostPrice { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SellingPrice { get; set; }

        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        public int LowStockThreshold { get; set; } = 10;

        [Required, StringLength(100)]
        public string Category { get; set; } = "General";
    }
}