using Microsoft.EntityFrameworkCore;
using RetailPOS.API.Data;
using RetailPOS.API.DTOs.Products;
using RetailPOS.API.Models;

namespace RetailPOS.API.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context) => _context = context;

        public async Task<ProductResponseDto> CreateAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Barcode = dto.Barcode,
                Description = dto.Description,
                CostPrice = dto.CostPrice,
                SellingPrice = dto.SellingPrice,
                StockQuantity = dto.StockQuantity,
                LowStockThreshold = dto.LowStockThreshold,
                Category = dto.Category,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<ProductResponseDto> GetByIdAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? throw new KeyNotFoundException("Product not found.") : MapToDto(product);
        }

        public async Task<List<ProductResponseDto>> GetAllAsync(string? category = null, bool? lowStock = null)
        {
            var query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(p => p.Category == category);

            if (lowStock == true)
                query = query.Where(p => p.StockQuantity <= p.LowStockThreshold);

            var products = await query.OrderBy(p => p.Name).ToListAsync();
            return products.Select(MapToDto).ToList();
        }

        public async Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) throw new KeyNotFoundException("Product not found.");

            if (dto.Name != null) product.Name = dto.Name;
            if (dto.Barcode != null) product.Barcode = dto.Barcode;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.CostPrice.HasValue) product.CostPrice = dto.CostPrice.Value;
            if (dto.SellingPrice.HasValue) product.SellingPrice = dto.SellingPrice.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.LowStockThreshold.HasValue) product.LowStockThreshold = dto.LowStockThreshold.Value;
            if (dto.Category != null) product.Category = dto.Category;

            product.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return MapToDto(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ProductExistsAsync(int id) => await _context.Products.AnyAsync(p => p.Id == id);

        private static ProductResponseDto MapToDto(Product p) => new()
        {
            Id = p.Id,
            Name = p.Name,
            Barcode = p.Barcode,
            Description = p.Description,
            CostPrice = p.CostPrice,
            SellingPrice = p.SellingPrice,
            StockQuantity = p.StockQuantity,
            Category = p.Category,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        };
    }
}
