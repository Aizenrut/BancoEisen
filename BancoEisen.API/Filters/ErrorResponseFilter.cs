using BancoEisen.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace BancoEisen.API.Filters
{
    public class ErrorResponseFilter : IExceptionFilter
    {
        private readonly ILogger logger;

        public ErrorResponseFilter(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ErrorResponseFilter>();
        }

        public void OnException(ExceptionContext context)
        {
            context.Result = new ObjectResult(ErrorResponse.From(context.Exception)) { StatusCode = 500 };
            context.ExceptionHandled = true;

            logger.LogError($"Erro: { context.Exception.Message }", context.Exception);
        }
    }
}
