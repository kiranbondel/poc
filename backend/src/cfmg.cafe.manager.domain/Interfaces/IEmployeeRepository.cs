using System.Collections.Generic;
using System.Threading.Tasks;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;
using Cfmg.Cafe.Manager.Domain.DomainModels;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Domain.Interfaces
{
    public interface IEmployeeRepository : IRepository
    {
        Task<bool> AddAsync(EmployeeEntity entity);
        Task<bool> UpdateAsync(EmployeeEntity entity);
        Task<bool> DeleteAsync(EmployeeEntity entity);
        Task<string?> GetLastIdAsync();
        Task<EmployeeEntity?> GetByIdAsync(string Id);
        Task<List<EmployeeDto>> GetAllAsync(string? cafeName);
    }
}
