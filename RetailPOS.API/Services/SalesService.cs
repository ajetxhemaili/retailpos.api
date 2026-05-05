using Microsoft.EntityFrameworkCore;
using RetailPOS.API.Data;
using RetailPOS.API.DTOs.Sales;
using RetailPOS.API.Models;

namespace RetailPOS.API.Services
{
    public class SalesService : ISalesService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;

        public SalesService(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<SaleResponseDto> CreateSaleAsync(int cashierId, CreateSaleDto dto)
        {
            var taxRate = decimal.Parse(_config["AppSettings:TaxRate"] ?? "0.18");
            decimal subTotal = 0;
            var items = new List<SaleItem>();

            // 1. Validate stock & calculate totals
            foreach (var itemDto in dto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId)
                    ?? throw new KeyNotFoundException($"Product {itemDto.ProductId} not found.");

                if (product.StockQuantity < itemDto.Quantity)
                    throw new InvalidOperationException($"Insufficient stock for {product.Name}. Available: {product.StockQuantity}");

                var lineTotal = product.SellingPrice * itemDto.Quantity;
                subTotal += lineTotal;

                items.Add(new SaleItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.SellingPrice,
                    LineTotal = lineTotal
                });

                // Deduct stock immediately (EF Core tracks it)
                product.StockQuantity -= itemDto.Quantity;
                _context.StockHistories.Add(new StockHistory
                {
                    ProductId = product.Id,
                    QuantityChange = -itemDto.Quantity,
                    Reason = "Sale",
                    ChangedByUserId = cashierId,
                    ChangedAt = DateTime.UtcNow
                });
            }

            // 2. Create Sale Header
            var sale = new Sale
            {
                ReceiptNumber = $"REC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}",
                SubTotal = subTotal,
                TaxAmount = Math.Round(subTotal * taxRate, 2),
                DiscountAmount = 0,
                TotalAmount = Math.Round(subTotal + (subTotal * taxRate), 2),
                PaymentMethod = dto.PaymentMethod,
                CashierId = cashierId,
                SaleDate = DateTime.UtcNow,
                SaleItems = items
            };

            _context.Sales.Add(sale);
            await _context.SaveChangesAsync();

            // 3. Map to Response DTO
            return new SaleResponseDto
            {
                Id = sale.Id,
                ReceiptNumber = sale.ReceiptNumber,
                SubTotal = sale.SubTotal,
                TaxAmount = sale.TaxAmount,
                DiscountAmount = sale.DiscountAmount,
                TotalAmount = sale.TotalAmount,
                PaymentMethod = sale.PaymentMethod,
                CashierId = sale.CashierId,
                SaleDate = sale.SaleDate,
                Items = items.Select(i => new SaleItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                }).ToList()
            };
        }

        public async Task<SaleResponseDto> GetSaleByIdAsync(int id)
        {
            var sale = await _context.Sales
                .Include(s => s.SaleItems).ThenInclude(si => si.Product)
                .FirstOrDefaultAsync(s => s.Id == id)
                ?? throw new KeyNotFoundException("Sale not found.");

            return new SaleResponseDto
            {
                Id = sale.Id,
                ReceiptNumber = sale.ReceiptNumber,
                SubTotal = sale.SubTotal,
                TaxAmount = sale.TaxAmount,
                DiscountAmount = sale.DiscountAmount,
                TotalAmount = sale.TotalAmount,
                PaymentMethod = sale.PaymentMethod,
                CashierId = sale.CashierId,
                SaleDate = sale.SaleDate,
                Items = sale.SaleItems.Select(i => new SaleItemResponseDto
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    LineTotal = i.LineTotal
                }).ToList()
            };
        }
    }
}
