using Weather.API.Configuration;
using Weather.API.EndpointBuilders;
using Weather.API.Middlewares;
using Weather.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogging();

builder.Services.AddDatabase();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.BuildWeatherEndpoints();

app.Run();