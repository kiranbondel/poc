using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Application.Interfaces;
using Cfmg.Cafe.Manager.Common.Library;
using Cfmg.Cafe.Manager.Common.Library.Exceptions;
using Cfmg.Cafe.Manager.Domain.Interfaces;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;
using Cfmg.Cafe.Manager.Domain.DomainModels;

namespace Cfmg.Cafe.Manager.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICafeRepository _cafeRepository;
        private readonly ILogger<EmployeeService> _logger;

        public EmployeeService(
            IEmployeeRepository employeeRepository,
            ICafeRepository cafeRepository,
            ILogger<EmployeeService> logger)
        {
            _employeeRepository = employeeRepository;
            _cafeRepository = cafeRepository;
            _logger = logger;
        }

        public async Task<bool> AddAsync(EmployeeRequestDto employeeDto)
        {
            _logger.LogInformation("---Start in AddAsync");

            var result = false;
            Guid cafeId = Guid.Empty;

            if (!string.IsNullOrEmpty(employeeDto.Cafe))
            {
                var cafe = await _cafeRepository.GetByNameAsync(employeeDto.Cafe);
                if (cafe == null)
                {
                    throw new NotFoundException(string.Format("Cafe {0} not found", employeeDto.Cafe));
                }
                
                cafeId = cafe.Id;                
            }

            var emp = await _employeeRepository.GetLastIdAsync();
            
            var employee = new EmployeeEntity()
            {
                Id = EmployeeIdGenerator.GenerateEmployeeId(emp),
                Name = employeeDto.Name,
                EmailAddress = employeeDto.EmailAddress,
                Gender = employeeDto.Gender,
                PhoneNumber = employeeDto.PhoneNumber,
                CafeId = cafeId != Guid.Empty ? cafeId : null,
                StartDate = cafeId != Guid.Empty ? DateTime.Now : null,
            };

            result = await _employeeRepository.AddAsync(employee);

            _logger.LogInformation("---End in AddAsync");
            return result;
        }

        public async Task<bool> UpdateAsync(EmployeeRequestDto employeeDto)
        {
            _logger.LogInformation("---Start in UpdateAsync");

            var result = false;
            Guid cafeId = Guid.Empty;

            var employee = await _employeeRepository.GetByIdAsync(employeeDto.Id);
            if (employee == null)
            {
                throw new NotFoundException(string.Format("Employee {0} not found", employeeDto.Id));
            }

            if (!string.IsNullOrEmpty(employeeDto.Cafe))
            {
                var cafe = await _cafeRepository.GetByNameAsync(employeeDto.Cafe);
                if (cafe == null)
                {
                    throw new NotFoundException(string.Format("Cafe {0} not found", employeeDto.Cafe));
                }

                cafeId = cafe.Id;
            }

            employee.Name = employeeDto.Name;
            employee.EmailAddress = employeeDto.EmailAddress;
            employee.Gender = employeeDto.Gender;
            employee.PhoneNumber = employeeDto.PhoneNumber;
            employee.StartDate = cafeId != Guid.Empty ? cafeId.Equals(employee.CafeId)? employee.StartDate : DateTime.Now : null;
            employee.CafeId = cafeId != Guid.Empty ? cafeId : null;


            result = await _employeeRepository.UpdateAsync(employee);

            _logger.LogInformation("---End in UpdateAsync");
            return result;
        }

        public Task<List<EmployeeDto>> GetEmployeesByCafeAsync(string? cafe)
        {
            _logger.LogInformation("---Start in GetEmployeesAsync");

            var result = _employeeRepository.GetAllAsync(cafe);

            _logger.LogInformation("---End in GetEmployeesAsync");
            return result;
        }

        public async Task<bool> DeleteAsync(string employeeId)
        {
            _logger.LogInformation("---Start in DeleteAsync");

            var result = false;
            Guid cafeId = Guid.Empty;

            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
            {
                throw new NotFoundException(string.Format("Employee {0} not found", employeeId));
            }

            result = await _employeeRepository.DeleteAsync(employee);

            _logger.LogInformation("---End in DeleteAsync");
            return result;
        }
    }
}
