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
    public class CafeRepository : ICafeRepository
    {
        private readonly CafeManagerDbContext _dbContext;

        public CafeRepository(CafeManagerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<bool> AddAsync(CafeEntity entity)
        {
            await _dbContext.Cafe.AddAsync(entity);
            return await SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(CafeEntity entity)
        {
            _dbContext.Cafe.Update(entity);
            return await SaveChangesAsync();
        }

        public async Task<CafeEntity?> GetByNameAsync(string cafe)
        {
            return await _dbContext.Cafe.FirstOrDefaultAsync(c => c.IsActive && c.Name == cafe);
        }

        public async Task<CafeEntity?> GetByIdAsync(Guid Id)
        {
            return await _dbContext.Cafe.FirstOrDefaultAsync(c => c.IsActive && c.Id == Id);
        }

        public async Task<List<CafeDto>> GetByLocationAsync(string? cafeLocation)
        {
            var query = _dbContext.Cafe.Where(c => c.IsActive);

            if (!string.IsNullOrEmpty(cafeLocation))
            {
                query = query.Where(c => c.Location == cafeLocation);
            }

            var cafes = await query
                .Select(c => new CafeDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Location = c.Location,
                    Logo = c.Logo??string.Empty,
                    NoOfEmployees = c.Employees.Count()
                })
            .ToListAsync();

            return cafes;
        }

        public async Task<bool> DeleteAsync(CafeEntity entity)
        {
            _dbContext.Cafe.Remove(entity);
            return await SaveChangesAsync();
        }
    }

}
