using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;

namespace BancoEisen.API.Models
{
    public class Error
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public string Target { get; set; }
        public Error[] Details { get; set; }
        public Error InnerError { get; set; }

        public Error()
        {

        }

        private Error(string code, string message, string target, Error[] details, Error innerError)
        {
            Code = code;
            Message = message;
            Target = target;
            Details = details;
            InnerError = innerError;
        }

        public static Error From(Exception exception)
        {
            if (exception == null)
                return null;

            return new Error(code: exception.HResult.ToString(),
                             message: exception.Message,
                             target: exception.StackTrace,
                             details: null,
                             innerError: From(exception.InnerException));
        }

        public static Error From(ModelStateDictionary modelState)
        {
            if (modelState == null)
                return null;

            var details = modelState.Values.SelectMany(x => x.Errors)
                                           .Select(x => new Error("400", x.ErrorMessage, null, null, null))
                                           .ToArray();

            return new Error(code: "400",
                             message: "O conteúdo enviado na requisição é inválido",
                             target: null,
                             details: details,
                             innerError: null);
        }
    }
}
