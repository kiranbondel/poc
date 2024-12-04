using Cfmg.Cafe.Manager.Common.Library.Exceptions;
using Cfmg.Cafe.Manager.Common.Library.SeedWork.Response;
using Microsoft.AspNetCore.Mvc;

namespace Cfmg.Cafe.Manager.Common.Library.BaseClass
{
    public abstract class AbstractController : ControllerBase
    {

        protected ResponseDto PrepareResponse(bool success, string servicename, string returnCode, List<ExceptionDto> errors, object payload)
        {
            ResultDto item = new ResultDto(servicename, returnCode, errors, payload);
            return new ResponseDto(success, new List<ResultDto> { item });
        }

        protected ResponseDto PrepareResponse(bool success, List<ResultDto> results)
        {
            return new ResponseDto(success, results);
        }


        protected ObjectResult PopulateException(Exception ex, string actionMethodName)
        {

            if (ex is ArgumentException)
            {
                ResponseDto value = PrepareResponse(success: false, actionMethodName, "02", new List<ExceptionDto>
                {
                   new ExceptionDto("02", ex.Message, actionMethodName),
                }, new List<object>());
                return StatusCode(400, value);
            }

            if (ex is FluentValidationException)
            {
                ResponseDto value = PrepareResponse(success: false, actionMethodName, "02", DataValidationHelper.GetExceptionMessageList(ex.Message, actionMethodName), new List<object>());
                return StatusCode(400, value);
            }

            ResponseDto responseDto = PrepareResponse(success: false, actionMethodName, "99", new List<ExceptionDto>
                {
                   new ExceptionDto("99", ex.Message, actionMethodName),
                }, new List<object>());
            return StatusCode(500, responseDto);
        }

    }
}
