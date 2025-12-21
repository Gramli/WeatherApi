using System.Net;
using System.Text.Json;
using Weather.Core.HandlerModel;

namespace Weather.API.Middlewares
{
    public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        private readonly RequestDelegate _next = next ?? throw new ArgumentNullException(nameof(next));
        protected readonly ILogger<ExceptionMiddleware> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception generalEx)
            {
                _logger.LogError(generalEx, "Unexpected Error Occurred.");
                await WriteResponseAsync(generalEx, context);
            }
        }

        /// <summary>
        /// Write data to context response
        /// </summary>
        protected virtual async Task WriteResponseAsync(Exception generalEx, HttpContext context)
        {
            var (responseCode, responseMessage) = ExtractFromException(generalEx);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)responseCode;
            var jsonResult = CreateResponseJson(responseMessage);
            await context.Response.WriteAsync(jsonResult);
        }

        /// <summary>
        /// Create response object and serialize it to JSON
        /// </summary>
        protected virtual string CreateResponseJson(string errorMessage)
        {
            var response = new DataResponse<object>()
            {
                Errors = [errorMessage]
            };
            return JsonSerializer.Serialize(response);
        }

        protected virtual (HttpStatusCode responseCode, string responseMessage) ExtractFromException(Exception generalEx)
            => generalEx switch
            {
                TaskCanceledException taskCanceledException => (HttpStatusCode.NoContent, taskCanceledException.Message),
                _ => (HttpStatusCode.InternalServerError, "Generic error occurred on server. Check logs for more info.")
            };
    }
}
