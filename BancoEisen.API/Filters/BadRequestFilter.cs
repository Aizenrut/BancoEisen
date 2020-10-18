using BancoEisen.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace BancoEisen.API.Filters
{
    public class BadRequestFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentException || context.Exception is InvalidOperationException)
            {
                context.Result = new ObjectResult(ErrorResponse.From(context.Exception)) { StatusCode = 400 };
                context.ExceptionHandled = true;
            }
        }
    }
}
