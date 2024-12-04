using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Application.Interfaces
{
    public interface IEmployeeService
    {
        Task<bool> AddAsync(EmployeeRequestDto employee);
        Task<bool> UpdateAsync(EmployeeRequestDto employee);
        Task<bool> DeleteAsync(string employeeId);
        Task<List<EmployeeDto>> GetEmployeesByCafeAsync(string cafe);
    }
}
