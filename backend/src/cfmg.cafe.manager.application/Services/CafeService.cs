using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Application.Interfaces;
using Cfmg.Cafe.Manager.Common.Library.Exceptions;
using Cfmg.Cafe.Manager.Domain.Interfaces;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;
using Cfmg.Cafe.Manager.Domain.DomainModels;

namespace Cfmg.Cafe.Manager.Application.Services
{
    public class CafeService : ICafeService
    {
        private readonly ICafeRepository _cafeRepository;
        private readonly ILogger<CafeService> _logger;

        public CafeService(
            ICafeRepository cafeRepository,
            ILogger<CafeService> logger)
        {
            _cafeRepository = cafeRepository;
            _logger = logger;
        }

        public async Task<bool> AddAsync(CafeRequestDto cafeDto)
        {
            _logger.LogInformation("---Start in AddAsync");

            var Cafe = await _cafeRepository.GetByNameAsync(cafeDto.Name);
            if(Cafe!=null)
                throw new FluentValidationException(string.Format("Name Already exist with same name.", cafeDto.Name));
            Cafe = new CafeEntity()
            {
                Name = cafeDto.Name,
                Description = cafeDto.Description,
                Location = cafeDto.Location,
                Logo = cafeDto.Logo
            };

            var result = await _cafeRepository.AddAsync(Cafe);

            _logger.LogInformation("---End in AddAsync");
            return result;
        }

        public async Task<bool> UpdateAsync(CafeRequestDto cafeDto)
        {
            _logger.LogInformation("---Start in UpdateAsync");

            var Cafe = await _cafeRepository.GetByIdAsync(Guid.Parse(cafeDto.Id)) ?? throw new NotFoundException(string.Format("Cafe {0} not found", cafeDto.Id));
            var existCafe = await _cafeRepository.GetByNameAsync(cafeDto.Name);
            if(existCafe != null && !Cafe.Id.Equals(existCafe.Id))
                throw new FluentValidationException(string.Format("Name Already exist with same name.", cafeDto.Name));

            Cafe.Name = cafeDto.Name;
            Cafe.Description = cafeDto.Description;
            Cafe.Location = cafeDto.Location;
            Cafe.Logo = cafeDto.Logo;
            
            var result = await _cafeRepository.UpdateAsync(Cafe);

            _logger.LogInformation("---End in UpdateAsync");
            return result;
        }

        public async Task<List<CafeDto>> GetCafesByLocationAsync(string? location)
        {
            _logger.LogInformation("---Start in GetCafesAsync");

            var result = await _cafeRepository.GetByLocationAsync(location);

            _logger.LogInformation("---End in GetCafesAsync");
            return result;
        }

        public async Task<bool> DeleteAsync(string cafeId)
        {
            _logger.LogInformation("---Start in DeleteAsync");

            var cafe = await _cafeRepository.GetByIdAsync(Guid.Parse(cafeId)) ?? throw new NotFoundException(string.Format("Cafe {0} not found", cafeId));            
            
            var result = await _cafeRepository.DeleteAsync(cafe);

            _logger.LogInformation("---End in DeleteAsync");
            return result;
        }
    }
}
