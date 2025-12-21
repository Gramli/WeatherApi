using Scalar.AspNetCore;
using Weather.API.Configuration;
using Weather.API.EndpointBuilders;
using Weather.API.Middlewares;
using Weather.Core.Configuration;
using Weather.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCore();

builder.Services.AddOpenApi();

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

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.BuildWeatherEndpoints();

app.Run();