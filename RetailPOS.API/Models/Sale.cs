using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RetailPOS.API.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string ReceiptNumber { get; set; } = string.Empty; // REC-YYYYMMDD-001

        [Column(TypeName = "decimal(18,2)")]
        public decimal SubTotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; } = "Cash"; // Cash, Card, Mobile

        public int CashierId { get; set; }

        [ForeignKey("CashierId")]
        public User? Cashier { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.UtcNow;

        public List<SaleItem> SaleItems { get; set; } = new();
    }
}
