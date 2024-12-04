using Cfmg.Cafe.Manager.Common.Library.SeedWork.Response;

namespace Cfmg.Cafe.Manager.Common.Library
{
    public static class DataValidationHelper
    {
        public static List<ExceptionDto> GetExceptionMessageList(string exceptionMessage, string actionMethod)
        {
            List<ExceptionDto> list = new List<ExceptionDto>();
            string[] array = exceptionMessage.Split(Environment.NewLine);
            foreach (var text in array)
            {
                if (!string.IsNullOrEmpty(text))
                {
                    ExceptionDto item = new ExceptionDto("02", text, actionMethod);
                    list.Add(item);
                }
            }

            return list;
        }

    }
}
