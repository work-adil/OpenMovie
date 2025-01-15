using Carter;
using MediatR;
using Microsoft.OpenApi.Models;
using OpenMovie.Application.CQRS.Models.Queries;
using OpenMovie.Application.CQRS.Models.Responses;

namespace OpenMovie.API.Endpoints
{
    public class MoviesModule : CarterModule
    {
        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/movies", async (IMediator mediator, string s, string apikey) =>
            {   
                var query = new GetMoviesQuery { Title = s, ApiKey = apikey };
                var result = await mediator!.Send(query);

                if (result.Result == null || !result.Result.Any())
                    return Results.NotFound($"Movie with title '{s}' not found.");

                return Results.Ok(result);
            })
            .WithSummary("Get movies by title")  
            .WithDescription("This endpoint fetches a movies by its title and returns the movie details like title, release year, and poster.")            
            .Produces<MovieResponse>(200)  
            .Produces(400)  
            .Produces(404)
            .WithOpenApi(options =>
            {
                // Explicitly defining parameters for Swagger documentation
                options.Parameters = new List<OpenApiParameter>
                {
                    new OpenApiParameter
                    {
                        Name = "s",
                        In = ParameterLocation.Query,
                        //Required = true,
                        Description = "The title of the movie to search for."
                    },
                    new OpenApiParameter
                    {
                        Name = "apikey",
                        In = ParameterLocation.Query,
                        //Required = true,
                        Description = "The API key to access the movie information from the external service."
                    }
                };
                return options;
            });
        }
    }
}
