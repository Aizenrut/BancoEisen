using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace BancoEisen.AuthProvider.Models
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

        public static ErrorResponse From(IEnumerable<IdentityError> identityErrors)
        {
            return new ErrorResponse(Error.From(identityErrors));
        }
    }
}
