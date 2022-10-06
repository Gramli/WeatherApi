using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Weather.API.Extensions
{
    internal static class MyResults
    {
        public static IActionResult StatusCode(HttpStatusCode statusCode, object value)
            => new StatusCodeObjectResult(statusCode, value);
    }
}
