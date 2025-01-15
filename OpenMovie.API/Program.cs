using Carter;
using Microsoft.OpenApi.Models;
using OpenMovie.Application.Services;
using OpenMovie.Core.Interfaces;
using OpenMovie.Infrastructure.Interfaces;
using OpenMovie.Infrastructure.Services;
using OpenMovie.Application.CQRS.Handlers.Extensions;
using Microsoft.AspNetCore.Diagnostics;
using OpenMovie.Application.CQRS.Models.Responses;
using FluentValidation;
using System.Text.Json;
using System.Net;
using OpenMovie.API;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Open Movie API",
                    Version = "v1",
                    Description = "API for fetching movie information"
                });
            })
            .AddScoped<IMovieService, MovieService>()
            .AddMediatRServices()
            .AddCarter()
            .AddHttpClient<IMovieApiClient, OmdbApiClient>();

        // Enable CORS based on configuration
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins(builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>().AllowedOrigins);
                });
        });

        var app = builder.Build();
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors(x => x
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            .WithOrigins(builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>().AllowedOrigins));

        app.UseHttpsRedirection();
        app.MapCarter();
        app.UseExceptionHandler(appBuilder =>
        {
            appBuilder.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                if (exceptionHandlerPathFeature?.Error is ValidationException validationException)
                {
                    // Handle validation exceptions (e.g., from FluentValidation)
                    context.Response.StatusCode = 400; // Bad Request
                    var errorResponse = new BaseResult
                    {
                        ResponseStatusCode = HttpStatusCode.BadRequest,
                        Message = "Validation failed",
                        Errors = validationException.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}").ToList()
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
                else if (exceptionHandlerPathFeature?.Error is OpenMovieException openMovieException)
                {
                    context.Response.StatusCode = 400; // Bad Request
                    var errorResponse = new BaseResult
                    {
                        ResponseStatusCode = HttpStatusCode.BadRequest,
                        Message = $"Application Error: {openMovieException.ErrorCode}: {openMovieException.Message}",
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
                else
                { 
                    context.Response.StatusCode = 500; // Internal Server Error
                    var errorResponse = new BaseResult
                    {
                        ResponseStatusCode = HttpStatusCode.InternalServerError,
                        Message = "An unexpected error occurred",
                    };
                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                }
            });
        });
        app.Run();
    }
}

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
