using Microsoft.AspNetCore.Mvc;
using Cfmg.Cafe.Manager.Common.Library.BaseClass;
using Cfmg.Cafe.Manager.Common.Library.SeedWork.Response;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;
using Cfmg.Cafe.Manager.Application.Interfaces;
using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Application.Validators;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Application.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CafeController : AbstractController
    {
        private readonly ILogger<CafeController> _logger;
        private readonly ICafeService _CafeService;

        private readonly string controllername = nameof(CafeController);
        private string actionmethodName = string.Empty;

        public CafeController(ILogger<CafeController> logger, ICafeService CafeService)
        {
            _logger = logger;
            _CafeService = CafeService;
        }


        [HttpGet("/Cafe")]
        [ProducesResponseType(typeof(CafeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetCafes([FromQuery] string? location)
        {
            actionmethodName = nameof(GetCafes);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                var result = await _CafeService.GetCafesByLocationAsync(location);
                ResponseDto reponseDto = PrepareResponse(result != null,
                                    actionmethodName,
                                    ReturnCode.Success,
                                    new List<ExceptionDto>(),
                                    result
                                    );

                return Ok(reponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    LoggerConstant.LogError,
                    ex.Message,
                    controllername,
                    actionmethodName,
                    ex.StackTrace);

                return PopulateException(ex, actionmethodName);
            }

        }

        [HttpPost("/Cafe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AddCafe(CafeRequestDto requestDto)
        {
            actionmethodName = nameof(AddCafe);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ValidatorExtension.Validate<CafeRequestDto, CafeRequestDtoValidator>(requestDto);
                ResponseDto reponseDto = PrepareResponse(await _CafeService.AddAsync(requestDto),
                                    actionmethodName,
                                    ReturnCode.Success,
                                    new List<ExceptionDto>(),
                                    string.Empty);

                return Ok(reponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    LoggerConstant.LogError,
                    ex.Message,
                    controllername,
                    actionmethodName,
                    ex.StackTrace);

                return PopulateException(ex, actionmethodName);
            }

        }

        [HttpPut("/Cafe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateCafe(CafeRequestDto requestDto)
        {
            actionmethodName = nameof(UpdateCafe);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ValidatorExtension.Validate<CafeRequestDto, CafeRequestDtoValidator>(requestDto);
                ResponseDto reponseDto = PrepareResponse(await _CafeService.UpdateAsync(requestDto),
                                    actionmethodName,
                                    ReturnCode.Success,
                                    new List<ExceptionDto>(),
                                    string.Empty);

                return Ok(reponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    LoggerConstant.LogError,
                    ex.Message,
                    controllername,
                    actionmethodName,
                    ex.StackTrace);

                return PopulateException(ex, actionmethodName);
            }

        }

        [HttpDelete("/Cafe")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteCafe(string Id)
        {
            actionmethodName = nameof(DeleteCafe);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ResponseDto reponseDto = PrepareResponse(await _CafeService.DeleteAsync(Id),
                                    actionmethodName,
                                    ReturnCode.Success,
                                    new List<ExceptionDto>(),
                                    string.Empty);

                return Ok(reponseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    LoggerConstant.LogError,
                    ex.Message,
                    controllername,
                    actionmethodName,
                    ex.StackTrace);

                return PopulateException(ex, actionmethodName);
            }

        }

        private static string GetFullActionMethodName(string controllername, string actionmethodName)
        {
            return $"{controllername}.{actionmethodName}";
        }
    }
}

