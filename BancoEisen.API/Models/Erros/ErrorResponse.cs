using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace BancoEisen.API.Models.Erros
{
    public class ErrorResponse
    {
        public Error Error { get; set; }

        public ErrorResponse()
        {

        }

        private ErrorResponse(Error error)
        {
            Error = error;
        }

        public static ErrorResponse From(Exception exception)
        {
            return new ErrorResponse(Error.From(exception));
        }

        public static ErrorResponse From(ModelStateDictionary modelState)
        {
            return new ErrorResponse(Error.From(modelState));
        }
    }
}
