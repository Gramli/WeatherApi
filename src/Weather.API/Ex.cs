using SmallApiToolkit.Middleware;
using System.Net;
using System.Runtime.CompilerServices;

namespace Weather.API
{
    public class Ex : ExceptionMiddleware
    {
        public Ex(RequestDelegate next, ILogger<ExceptionMiddleware> logger) : base(next, logger)
        {
        }

        protected override (HttpStatusCode responseCode, string responseMessage) ExtractFromException(Exception generalEx)
        {
            return base.ExtractFromException(generalEx);
        }
    }
}
