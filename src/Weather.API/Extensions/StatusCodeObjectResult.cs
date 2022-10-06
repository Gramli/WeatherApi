using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Weather.API.Extensions
{
    internal class StatusCodeObjectResult : ObjectResult
    {
        internal StatusCodeObjectResult(HttpStatusCode statusCode, object value) 
            : base(value)
        {
            StatusCode = (int)statusCode;
        }
    }
}
