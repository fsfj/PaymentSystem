using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PaymentSystem.Models
{

    //* I made this to  *
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }

            catch (Exception ex) {
                await HandleExceptionAsync(context, ex);
            }
        
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            var customeException = exception as BaseCustomException;
            var statusCode = (int)HttpStatusCode.InternalServerError;
            var message = exception.Message;
            var description = "Unexpected Error";

            if (null != customeException)
            {
                message = customeException.Message;
                description = customeException.Description;
                statusCode = customeException.Code;
            }

            response.ContentType = "application/json";
            response.StatusCode = statusCode;

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            await response.WriteAsync(JsonConvert.SerializeObject(new CustomMessage
            {
                Message = message,
                HasError = true,
                StatusCode = statusCode
                
            }, serializerSettings));
        }
    }

    public class CustomMessage { 
        public string Message { get; set; }
        public bool HasError { get; set; }

        public int StatusCode { get; set; }
    }
}
