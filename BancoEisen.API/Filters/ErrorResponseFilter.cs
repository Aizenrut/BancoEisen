using BancoEisen.API.Models.Erros;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BancoEisen.API.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(ErrorResponse.From(context.Exception)) { StatusCode = 500 };
            context.ExceptionHandled = true;
        }
    }
}
