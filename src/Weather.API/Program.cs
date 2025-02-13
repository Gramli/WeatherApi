using Scalar.AspNetCore;
using SmallApiToolkit.Extensions;
using SmallApiToolkit.Middleware;
using Weather.API.Configuration;
using Weather.API.EndpointBuilders;
using Weather.API.Middleware;
using Weather.Core.Configuration;
using Weather.Infrastructure.Configuration;


var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddApi(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();

builder.Services.AddOpenApi();

var corsPolicyName = builder.Services.AddCorsByConfiguration(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("Weather API")
            .WithTheme(ScalarTheme.Mars)
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseCors(corsPolicyName);

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggingMiddlewareWithOptions>();

app.BuildWeatherEndpoints();

app.Run();