﻿namespace Cfmg.Cafe.Manager.Common.Library.SeedWork.Response
{
    public class ResultDto
    {
        public string ServiceName { get; set; }
        public string ReturnCode { get; set; }
        public List<ExceptionDto> Errors { get; set; }
        public object Payload {  get; set; }

        public ResultDto(string serviceName, string returnCode, List<ExceptionDto> errors, object payload)
        {
            ServiceName = serviceName;
            ReturnCode = returnCode;
            Errors = errors;
            Payload = payload;
        }
    }
}