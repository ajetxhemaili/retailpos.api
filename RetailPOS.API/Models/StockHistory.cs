using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailPOS.API.Models
{
    public class StockHistory
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; } = null!;

        public int QuantityChange { get; set; } // + for restock, - for sale

        [Required, StringLength(100)]
        public string Reason { get; set; } = string.Empty; // "Sale", "Restock", "Adjustment"

        public int? SaleId { get; set; } // Nullable: null if restock

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        public int ChangedByUserId { get; set; }

        [ForeignKey("ChangedByUserId")]
        public User? ChangedBy { get; set; }
    }
}
