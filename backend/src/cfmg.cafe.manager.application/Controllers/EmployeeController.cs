using Microsoft.AspNetCore.Mvc;
using Cfmg.Cafe.Manager.Application.Interfaces;
using Cfmg.Cafe.Manager.Application.Models.Dto;
using Cfmg.Cafe.Manager.Application.Validators;
using Cfmg.Cafe.Manager.Common.Library.BaseClass;
using Cfmg.Cafe.Manager.Common.Library.SeedWork.Response;
using Cfmg.Cafe.Manager.Common.Library.SeedWork;
using Cfmg.Cafe.Manager.Domain.DomainModels.Dto;

namespace Cfmg.Cafe.Manager.Application.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class EmployeeController : AbstractController
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly IEmployeeService _employeeService;

        private readonly string controllername = nameof(EmployeeController);
        private string actionmethodName = string.Empty;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            _employeeService = employeeService;
        }


        [HttpGet("/employee")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetEmployees([FromQuery] string? cafe)
        {
            actionmethodName = nameof(GetEmployees);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                var result = await _employeeService.GetEmployeesByCafeAsync(cafe);
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

        [HttpPost("/employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> AddEmployee(EmployeeRequestDto requestDto)
        {
            actionmethodName = nameof(AddEmployee);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ValidatorExtension.Validate<EmployeeRequestDto, EmployeeRequestDtoValidator>(requestDto);
                ResponseDto reponseDto = PrepareResponse(await _employeeService.AddAsync(requestDto),
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

        [HttpPut("/employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateEmployee(EmployeeRequestDto requestDto)
        {
            actionmethodName = nameof(UpdateEmployee);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ValidatorExtension.Validate<EmployeeRequestDto, EmployeeRequestDtoValidator>(requestDto);
                ResponseDto reponseDto = PrepareResponse(await _employeeService.UpdateAsync(requestDto),
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

        [HttpDelete("/employee")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteEmployee(string Id)
        {
            actionmethodName = nameof(DeleteEmployee);
            var fullActionName = GetFullActionMethodName(controllername, actionmethodName);

            try
            {
                ResponseDto reponseDto = PrepareResponse(await _employeeService.DeleteAsync(Id),
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

        private string GetFullActionMethodName(string controllername, string actionmethodName)
        {
            return $"{controllername}.{actionmethodName}";
        }
    }
}

