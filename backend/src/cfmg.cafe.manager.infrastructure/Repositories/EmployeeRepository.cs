using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Cfmg.Cafe.Manager.Domain.DomainModels;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;
using Cfmg.Cafe.Manager.Domain.Interfaces;

namespace Cfmg.Cafe.Manager.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly CafeManagerDbContext _dbContext;

        public EmployeeRepository(CafeManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0 ? true : false;
        }

        public async Task<bool> AddAsync(EmployeeEntity entity)
        {
            await _dbContext.Employee.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(EmployeeEntity entity)
        {
            _dbContext.Employee.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<List<EmployeeDto>> GetAllAsync(string? cafeName)
        {
            var query = _dbContext.Employee
                                  .Where(c => c.IsActive);

            if (!string.IsNullOrEmpty(cafeName))
            {
                query = query.Where(c => c.Cafe.Name == cafeName);
            }

            DateTime today = DateTime.Now;
            var employees = await query.Select(e => new EmployeeDto
                                       {
                                           Id = e.Id,
                                           Name = e.Name,
                                           EmailAddress = e.EmailAddress,
                                           PhoneNumber = e.PhoneNumber,
                                           Gender = e.Gender,
                                           DaysWorked = e.StartDate.HasValue? EF.Functions.DateDiffDay(e.StartDate.Value, today) : 0,
                                           Cafe = !string.IsNullOrEmpty(e.Cafe.Name)? e.Cafe.Name :string.Empty
                                       })
                                       .OrderByDescending(e => e.DaysWorked)
                                       .ToListAsync();

            return employees;
        }

        public async Task<EmployeeEntity?> GetByIdAsync(string Id)
        {
            return await _dbContext.Employee.FirstOrDefaultAsync(e => e.IsActive && e.Id == Id);
        }

        public async Task<string?> GetLastIdAsync()
        {
            return await _dbContext.Employee.OrderByDescending(e => e.Id)
                      .Select(e => e.Id)
                      .FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(EmployeeEntity entity)
        {
            _dbContext.Employee.Remove(entity);
            return await SaveChangesAsync();
        }
    }
}
