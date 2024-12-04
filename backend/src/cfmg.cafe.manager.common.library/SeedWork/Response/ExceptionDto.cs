namespace Cfmg.Cafe.Manager.Common.Library.SeedWork.Response
{
    public class ExceptionDto
    {
        public string MessageCode { get; set; }
        public string DisplayMessage { get; set; }
        public string ServiceName { get; set; }

        public ExceptionDto(string messageCode, string displayMessage, string serviceName)
        {
            MessageCode = messageCode;
            DisplayMessage = displayMessage;
            ServiceName = serviceName;
        }
    }
}