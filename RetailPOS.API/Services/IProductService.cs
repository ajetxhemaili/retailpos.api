using RetailPOS.API.DTOs.Products;

namespace RetailPOS.API.Services
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<ProductResponseDto> GetByIdAsync(int id);
        Task<List<ProductResponseDto>> GetAllAsync(string? category = null, bool? lowStock = null);
        Task<ProductResponseDto> UpdateAsync(int id, UpdateProductDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ProductExistsAsync(int id);
    }
}
