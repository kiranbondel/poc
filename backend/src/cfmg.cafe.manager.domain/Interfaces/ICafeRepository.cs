using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;
using Cfmg.Cafe.Manager.Domain.DomainModels;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Domain.Interfaces
{
    public interface ICafeRepository : IRepository
    {
        Task<bool> AddAsync(CafeEntity entity);
        Task<bool> UpdateAsync(CafeEntity entity);
        Task<bool> DeleteAsync(CafeEntity entity);
        Task<CafeEntity?> GetByNameAsync(string cafe);
        Task<CafeEntity?> GetByIdAsync(Guid Id);
        Task<List<CafeDto>> GetByLocationAsync(string? cafeLocation);
    }
}
