using RetailPOS.API.DTOs.Sales;

namespace RetailPOS.API.Services
{
    public interface ISalesService
    {
        Task<SaleResponseDto> CreateSaleAsync(int cashierId, CreateSaleDto dto);
        Task<SaleResponseDto> GetSaleByIdAsync(int id);
    }
}