using Ardalis.GuardClauses;
using Microsoft.Extensions.Options;
using SmallApiToolkit.Middleware;
using Weather.API.Options;

namespace Weather.API.Middleware
{
    public class LoggingMiddlewareWithOptions : LoggingMiddleware
    {
        private readonly IOptionsMonitor<ApiLoggingOptions> _optionsMonitor;
        public LoggingMiddlewareWithOptions(IOptionsMonitor<ApiLoggingOptions> optionsMonitor, RequestDelegate next, ILogger<LoggingMiddleware> logger) 
            : base(next, logger)
        {
            _optionsMonitor = Guard.Against.Null(optionsMonitor);
        }

        protected override Task LogRequest(HttpRequest request)
        {
            if(_optionsMonitor.CurrentValue.LogRequest)
            {
                return base.LogRequest(request);
            }
            
            return Task.CompletedTask;
        }

        protected override Task LogResponse(HttpResponse response)
        {
            if (_optionsMonitor.CurrentValue.LogResponse)
            {
                return base.LogResponse(response);
            }
            return Task.CompletedTask;
        }
    }
}
