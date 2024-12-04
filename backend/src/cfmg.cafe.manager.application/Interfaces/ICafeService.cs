using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Application.Interfaces
{
    public interface ICafeService
    {
        Task<bool> AddAsync(CafeRequestDto cafe);
        Task<bool> UpdateAsync(CafeRequestDto Cafe);
        Task<bool> DeleteAsync(string cafeId);
        Task<List<CafeDto>> GetCafesByLocationAsync(string? location);
    }
}
